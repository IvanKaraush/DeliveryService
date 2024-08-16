using Domain.Models.ApplicationModels;
using System.Net;
using Newtonsoft.Json;

namespace API.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (InvalidFileFormatException iFFEx)
            {
                await HandleInvalidFileFormatException(context, iFFEx);
            }
            catch (InvalidPageException iPEx)
            {
                await HandleInvalidPageException(context, iPEx);
            }
            catch (DoesNotExistException dNEEx)
            {
                await HandleDoesNotExistException(context, dNEEx);
            }
            catch (InvalidMarkException iMEx)
            {
                await HandleInvalidMarkException(context, iMEx);
            }
            catch (WasAlreadySetException wASEx)
            {
                await HandleWasAlreadySetException(context, wASEx);
            }
            catch (InvalidCardNumberException iCNEx)
            {
                await HandleInvalidCardNumberException(context, iCNEx);
            }
            catch (Exception ex)
            {
                await HandleUnknownException(context, ex);
            }
        }
        private static Task HandleInvalidMarkException(HttpContext context, InvalidMarkException exception)
        {
            int statusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = exception.Message
            });
            return context.Response.WriteAsync(result);
        }
        private static Task HandleWasAlreadySetException(HttpContext context, WasAlreadySetException exception)
        {
            int statusCode = (int)HttpStatusCode.Forbidden;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = exception.Message
            });
            return context.Response.WriteAsync(result);
        }
        private static Task HandleInvalidCardNumberException(HttpContext context, InvalidCardNumberException exception)
        {
            int statusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = exception.Message
            });
            return context.Response.WriteAsync(result);
        }
        private static Task HandleInvalidFileFormatException(HttpContext context, InvalidFileFormatException exception)
        {
            int statusCode = (int)HttpStatusCode.UnsupportedMediaType;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = exception.Message
            });
            return context.Response.WriteAsync(result);
        }
        private static Task HandleInvalidPageException(HttpContext context, InvalidPageException exception)
        {
            int statusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = exception.Message
            });
            return context.Response.WriteAsync(result);
        }
        private static Task HandleDoesNotExistException(HttpContext context, DoesNotExistException exception)
        {
            int statusCode = (int)HttpStatusCode.NotFound;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = exception.Message
            });
            return context.Response.WriteAsync(result);
        }
        private static Task HandleUnknownException(HttpContext context, Exception exception)
        {
            int statusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = exception.Message
            });
            return context.Response.WriteAsync(result);
        }

    }

    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
