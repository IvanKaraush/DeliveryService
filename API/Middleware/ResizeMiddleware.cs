using Microsoft.Extensions.Caching.Memory;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using SkiaSharp;

namespace API.Middleware
{
    internal class ResizeMiddleware
    {
        struct ResizeParams
        {
            public bool HasParams;
            public int Width;
            public int Height;
            public bool Autorotate;
            public int Quality; // 0 - 100
            public string Format; // png, jpg, jpeg
            public string Mode;
            public static readonly string[] Modes = {
                "pad",
                "max",
                "crop",
                "stretch"
            };

            public override string ToString()
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"w: {Width}, ");
                stringBuilder.Append($"h: {Height}, ");
                stringBuilder.Append($"autorotate: {Autorotate}, ");
                stringBuilder.Append($"quality: {Quality}, ");
                stringBuilder.Append($"format: {Format}, ");
                stringBuilder.Append($"mode: {Mode}");

                return stringBuilder.ToString();
            }
        }

        [Obsolete]
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private readonly IMemoryCache _memoryCache;
        private readonly RequestDelegate _next;
        private static readonly string[] _suffixes = {
            "png",
            "jpg",
            "jpeg"
        };

        [Obsolete]
        public ResizeMiddleware(RequestDelegate next, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment, IMemoryCache memoryCache)
        {
            _next = next;
            _environment = environment;
            _memoryCache = memoryCache;
        }
        private bool IsImagePath(PathString path)
        {
            if (path == null || !path.HasValue)
                return false;

            return _suffixes.Any(x => x.EndsWith(x, StringComparison.OrdinalIgnoreCase));
        }

        [Obsolete]
        public async Task Invoke(HttpContext context)
        {
            PathString path = context.Request.Path;

            // hand to next middleware if we are not dealing with an image
            if (context.Request.Query.Count == 0 || !IsImagePath(path))
            {
                await _next.Invoke(context);
                return;
            }
            // hand to next middleware if we are dealing with an image but it doesn't have any usable resize querystring params
            ResizeParams resizeParams = GetResizeParams(path, context.Request.Query);
            if (!resizeParams.HasParams || (resizeParams.Width == 0 && resizeParams.Height == 0))
            {
                await _next.Invoke(context);
                return;
            }
            if (path.Value == null)
                throw new Exception("Request path is null");
            string imagePath = Path.Combine(
                _environment.WebRootPath,
                path.Value.Substring(5).Replace('/', Path.DirectorySeparatorChar).TrimStart(Path.DirectorySeparatorChar));

            // check file lastwrite
            DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(imagePath);
            if (lastWriteTimeUtc.Year == 1601) // file doesn't exist, pass to next middleware
            {
                await _next.Invoke(context);
                return;
            }

            SKData imageData = GetImageData(imagePath, resizeParams, lastWriteTimeUtc);

            // write to stream
            context.Response.ContentType = resizeParams.Format == "png" ? "image/png" : "image/jpeg";
            context.Response.ContentLength = imageData.Size;
            await context.Response.Body.WriteAsync(imageData.ToArray(), 0, (int)imageData.Size);

            // cleanup
            imageData.Dispose();
        }
        private ResizeParams GetResizeParams(PathString path, IQueryCollection query)
        {
            ResizeParams resizeParams = new ResizeParams();

            // before we extract, do a quick check for resize params
            resizeParams.HasParams =
                resizeParams.GetType().GetTypeInfo()
                .GetFields().Where(f => f.Name != "hasParams")
                .Any(f => query.ContainsKey(f.Name));

            // if no params present, bug out
            if (!resizeParams.HasParams)
                return resizeParams;

            // extract resize params

            if (path.Value == null)
                throw new Exception("Request path is null");
            if (query.ContainsKey("format"))
#pragma warning disable CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
                resizeParams.Format = query["format"];
#pragma warning restore CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
            else
                resizeParams.Format = path.Value.Substring(path.Value.LastIndexOf('.') + 1);

            if (query.ContainsKey("autorotate"))
                bool.TryParse(query["autorotate"], out resizeParams.Autorotate);

            int quality = 100;
            if (query.ContainsKey("quality"))
                int.TryParse(query["quality"], out quality);
            resizeParams.Quality = quality;

            int w = 0;
            if (query.ContainsKey("w"))
                int.TryParse(query["w"], out w);
            resizeParams.Width = w;

            int h = 0;
            if (query.ContainsKey("h"))
                int.TryParse(query["h"], out h);
            resizeParams.Height = h;

            resizeParams.Mode = "max";
            // only apply mode if it's a valid mode and both w and h are specified
            if (h != 0 && w != 0 && query.ContainsKey("mode") && ResizeParams.Modes.Any(m => query["mode"] == m))
#pragma warning disable CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
                resizeParams.Mode = query["mode"];
#pragma warning restore CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.

            return resizeParams;
        }

        [Obsolete]
        private SKData GetImageData(string imagePath, ResizeParams resizeParams, DateTime lastWriteTimeUtc)
        {
            // check cache and return if cached
            long cacheKey;
            unchecked
            {
                cacheKey = imagePath.GetHashCode() + lastWriteTimeUtc.ToBinary() + resizeParams.ToString().GetHashCode();
            }

            SKData imageData;
            byte[]? imageBytes;
            bool isCached = _memoryCache.TryGetValue(cacheKey, out imageBytes);
            if (isCached)
            {
                return SKData.CreateCopy(imageBytes);
            }

            SKEncodedOrigin origin; // this represents the EXIF orientation
            SKBitmap bitmap = LoadBitmap(File.OpenRead(imagePath), out origin); // always load as 32bit (to overcome issues with indexed color)

            // if autorotate = true, and origin isn't correct for the rotation, rotate it
            if (resizeParams.Autorotate && origin != SKEncodedOrigin.TopLeft)
                bitmap = RotateAndFlip(bitmap, origin);

            // if either w or h is 0, set it based on ratio of original image
            if (resizeParams.Height == 0)
                resizeParams.Height = (int)Math.Round(bitmap.Height * (float)resizeParams.Width / bitmap.Width);
            else if (resizeParams.Width == 0)
                resizeParams.Width = (int)Math.Round(bitmap.Width * (float)resizeParams.Height / bitmap.Height);

            // if we need to crop, crop the original before resizing
            if (resizeParams.Mode == "crop")
                bitmap = Crop(bitmap, resizeParams);

            // store padded height and width
            int paddedHeight = resizeParams.Height;
            int paddedWidth = resizeParams.Width;

            // if we need to pad, or max, set the height or width according to ratio
            if (resizeParams.Mode == "pad" || resizeParams.Mode == "max")
            {
                float bitmapRatio = (float)bitmap.Width / bitmap.Height;
                float resizeRatio = (float)resizeParams.Width / resizeParams.Height;

                if (bitmapRatio > resizeRatio) // original is more "landscape"
                    resizeParams.Height = (int)Math.Round(bitmap.Height * ((float)resizeParams.Width / bitmap.Width));
                else
                    resizeParams.Width = (int)Math.Round(bitmap.Width * ((float)resizeParams.Height / bitmap.Height));
            }

            // resize
            SKImageInfo resizedImageInfo = new SKImageInfo(resizeParams.Width, resizeParams.Height, SKImageInfo.PlatformColorType, bitmap.AlphaType);
            SKBitmap resizedBitmap = new SKBitmap(resizedImageInfo);
            bitmap.ScalePixels(resizedBitmap, SKFilterQuality.High);

            // optionally pad
            if (resizeParams.Mode == "pad")
                resizedBitmap = Pad(resizedBitmap, paddedWidth, paddedHeight, resizeParams.Format != "png");

            // encode
            SKImage resizedImage = SKImage.FromBitmap(resizedBitmap);
            SKEncodedImageFormat encodeFormat = resizeParams.Format == "png" ? SKEncodedImageFormat.Png : SKEncodedImageFormat.Jpeg;
            imageData = resizedImage.Encode(encodeFormat, resizeParams.Quality);

            // cache the result
            _memoryCache.Set(cacheKey, imageData.ToArray());

            // cleanup
            resizedImage.Dispose();
            bitmap.Dispose();
            resizedBitmap.Dispose();

            return imageData;
        }
        private SKBitmap Crop(SKBitmap original, ResizeParams resizeParams)
        {
            int cropSides = 0;
            int cropTopBottom = 0;

            // calculate amount of pixels to remove from sides and top/bottom
            if ((float)resizeParams.Width / original.Width < resizeParams.Height / original.Height) // crop sides
                cropSides = original.Width - (int)Math.Round((float)original.Height / resizeParams.Height * resizeParams.Width);
            else
                cropTopBottom = original.Height - (int)Math.Round((float)original.Width / resizeParams.Width * resizeParams.Height);

            // setup crop rect
            SKRectI cropRect = new SKRectI
            {
                Left = cropSides / 2,
                Top = cropTopBottom / 2,
                Right = original.Width - cropSides + cropSides / 2,
                Bottom = original.Height - cropTopBottom + cropTopBottom / 2
            };

            // crop
            SKBitmap bitmap = new SKBitmap(cropRect.Width, cropRect.Height);
            original.ExtractSubset(bitmap, cropRect);
            original.Dispose();

            return bitmap;
        }
        private SKBitmap Pad(SKBitmap original, int paddedWidth, int paddedHeight, bool isOpaque)
        {
            // setup new bitmap and optionally clear
            SKBitmap bitmap = new SKBitmap(paddedWidth, paddedHeight, isOpaque);
            SKCanvas canvas = new SKCanvas(bitmap);
            if (isOpaque)
                canvas.Clear(new SKColor(255, 255, 255)); // we could make this color a resizeParam
            else
                canvas.Clear(SKColor.Empty);

            // find co-ords to draw original at
            int left = original.Width < paddedWidth ? (paddedWidth - original.Width) / 2 : 0;
            int top = original.Height < paddedHeight ? (paddedHeight - original.Height) / 2 : 0;

            SKRectI drawRect = new SKRectI
            {
                Left = left,
                Top = top,
                Right = original.Width + left,
                Bottom = original.Height + top
            };

            // draw original onto padded version
            canvas.DrawBitmap(original, drawRect);
            canvas.Flush();

            canvas.Dispose();
            original.Dispose();

            return bitmap;
        }
        private SKBitmap RotateAndFlip(SKBitmap original, SKEncodedOrigin origin)
        {
            // these are the origins that represent a 90 degree turn in some fashion
            SKEncodedOrigin[] differentOrientations =
            {
                SKEncodedOrigin.LeftBottom,
                SKEncodedOrigin.LeftTop,
                SKEncodedOrigin.RightBottom,
                SKEncodedOrigin.RightTop
            };

            // check if we need to turn the image
            bool isDifferentOrientation = differentOrientations.Any(o => o == origin);

            // define new width/height
            int width = isDifferentOrientation ? original.Height : original.Width;
            int height = isDifferentOrientation ? original.Width : original.Height;

            SKBitmap bitmap = new SKBitmap(width, height, original.AlphaType == SKAlphaType.Opaque);

            // todo: the stuff in this switch statement should be rewritten to use pointers
            switch (origin)
            {
                case SKEncodedOrigin.LeftBottom:

                    for (var x = 0; x < original.Width; x++)
                        for (var y = 0; y < original.Height; y++)
                            bitmap.SetPixel(y, original.Width - 1 - x, original.GetPixel(x, y));
                    break;

                case SKEncodedOrigin.RightTop:

                    for (var x = 0; x < original.Width; x++)
                        for (var y = 0; y < original.Height; y++)
                            bitmap.SetPixel(original.Height - 1 - y, x, original.GetPixel(x, y));
                    break;

                case SKEncodedOrigin.RightBottom:

                    for (var x = 0; x < original.Width; x++)
                        for (var y = 0; y < original.Height; y++)
                            bitmap.SetPixel(original.Height - 1 - y, original.Width - 1 - x, original.GetPixel(x, y));

                    break;

                case SKEncodedOrigin.LeftTop:

                    for (var x = 0; x < original.Width; x++)
                        for (var y = 0; y < original.Height; y++)
                            bitmap.SetPixel(y, x, original.GetPixel(x, y));
                    break;

                case SKEncodedOrigin.BottomLeft:

                    for (var x = 0; x < original.Width; x++)
                        for (var y = 0; y < original.Height; y++)
                            bitmap.SetPixel(x, original.Height - 1 - y, original.GetPixel(x, y));
                    break;

                case SKEncodedOrigin.BottomRight:

                    for (var x = 0; x < original.Width; x++)
                        for (var y = 0; y < original.Height; y++)
                            bitmap.SetPixel(original.Width - 1 - x, original.Height - 1 - y, original.GetPixel(x, y));
                    break;

                case SKEncodedOrigin.TopRight:

                    for (var x = 0; x < original.Width; x++)
                        for (var y = 0; y < original.Height; y++)
                            bitmap.SetPixel(original.Width - 1 - x, y, original.GetPixel(x, y));
                    break;

            }

            original.Dispose();

            return bitmap;
        }
        private SKBitmap LoadBitmap(Stream stream, out SKEncodedOrigin origin)
        {
            using (SKManagedStream sKManagedStream = new SKManagedStream(stream))
            {
                using (SKCodec codec = SKCodec.Create(sKManagedStream))
                {
                    origin = codec.EncodedOrigin;
                    SKImageInfo info = codec.Info;
                    SKBitmap bitmap = new SKBitmap(info.Width, info.Height, SKImageInfo.PlatformColorType, info.IsOpaque ? SKAlphaType.Opaque : SKAlphaType.Premul);

                    IntPtr length;
                    SKCodecResult result = codec.GetPixels(bitmap.Info, bitmap.GetPixels(out length));
                    if (result == SKCodecResult.Success || result == SKCodecResult.IncompleteInput)
                    {
                        return bitmap;
                    }
                    else
                    {
                        throw new ArgumentException("Unable to load bitmap from provided data");
                    }
                }
            }
        }
    }
}