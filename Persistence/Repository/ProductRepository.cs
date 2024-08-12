using Domain.Models.ApplicationModels;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Persistence.Repository
{
    public class ProductRepository : IProductStore
    {
        public ProductRepository(SQLContext context, IDistributedCache cache, IOptions<ReposOptions> repositoryOptions) 
        {
            Context = context;
            Cache = cache;
            CacheOptions = new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(repositoryOptions.Value.CacheExpirationMins) };
        }
        private readonly SQLContext Context;
        private readonly IDistributedCache Cache;
        private readonly DistributedCacheEntryOptions CacheOptions = new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(5) };
        public async Task AddProduct(Product product)
        {
            await Context.Goods.AddAsync(product);
            await Context.SaveChangesAsync();
            await Cache.SetStringAsync(product.Article.ToString(), JsonConvert.SerializeObject(product), CacheOptions);
        }

        public async Task EditPrice(int article, decimal price)
        {
            Product? product;
            string? productString = await Cache.GetStringAsync(article.ToString());
            if (productString != null)
            {
                product = JsonConvert.DeserializeObject<Product>(productString);
                await Cache.RemoveAsync(article.ToString());
            }
            else
                product = await Context.Goods.FirstOrDefaultAsync(p => p.Article == article);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            product.Price = price;
            Context.Goods.Update(product);
            await Context.SaveChangesAsync();
            await Cache.SetStringAsync(product.Article.ToString(), JsonConvert.SerializeObject(product), CacheOptions);
        }

        public async Task Hide(int article)
        {
            Product? product;
            string? productString = await Cache.GetStringAsync(article.ToString());
            if (productString != null)
            {
                product = JsonConvert.DeserializeObject<Product>(productString);
                await Cache.RemoveAsync(article.ToString());
            }
            else
                product = await Context.Goods.FirstOrDefaultAsync(p => p.Article == article);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            product.Visible = false;
            Context.Goods.Update(product);
            await Context.SaveChangesAsync();
        }

        public async Task RemoveProduct(int article)
        {
            Product? product;
            string? productString = await Cache.GetStringAsync(article.ToString());
            if (productString != null)
            {
                product = JsonConvert.DeserializeObject<Product>(productString);
                await Cache.RemoveAsync(article.ToString());
            }
            else
                product = await Context.Goods.FirstOrDefaultAsync(p => p.Article == article);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            Context.Goods.Remove(product);
            await Context.SaveChangesAsync();
        }

        public async Task Show(int article)
        {
            Product? product;
            string? productString = await Cache.GetStringAsync(article.ToString());
            if (productString != null)
            {
                product = JsonConvert.DeserializeObject<Product>(productString);
                await Cache.RemoveAsync(article.ToString());
            }
            else
                product = await Context.Goods.FirstOrDefaultAsync(p => p.Article == article);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            product.Visible = true;
            Context.Goods.Update(product);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateCookingTime(int article, TimeOnly lastCookingTime)
        {
            Product? product;
            string? productString = await Cache.GetStringAsync(article.ToString());
            if (productString != null)
            {
                product = JsonConvert.DeserializeObject<Product>(productString);
                await Cache.RemoveAsync(article.ToString());
            }
            else
                product = await Context.Goods.FirstOrDefaultAsync(p => p.Article == article);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            if (!product.AverageCookingTime.HasValue)
            {
                product.AverageCookingTime = lastCookingTime;
                product.AlreadyCooked = 1;
            }
            int alreadyCooked = product.AlreadyCooked;
            long totalTicks = product.AverageCookingTime.Value.Ticks * alreadyCooked + lastCookingTime.Ticks;
            product.AverageCookingTime = new TimeOnly(totalTicks / ++alreadyCooked);
            product.AlreadyCooked = alreadyCooked;
            Context.Goods.Update(product);
            await Context.SaveChangesAsync();
            await Cache.SetStringAsync(product.Article.ToString(), JsonConvert.SerializeObject(product), CacheOptions);
        }

        public async Task UpdateRating(int article, int mark)
        {
            Product? product;
            string? productString = await Cache.GetStringAsync(article.ToString());
            if (productString != null)
            {
                product = JsonConvert.DeserializeObject<Product>(productString);
                await Cache.RemoveAsync(article.ToString());
            }
            else
                product = await Context.Goods.FirstOrDefaultAsync(p => p.Article == article);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            if (!product.Rating.HasValue)
            {
                product.Rating = mark;
                product.AlreadyRated = 1;
            }
            int alreadyRated = product.AlreadyRated;
            float totalRating = product.Rating.Value * alreadyRated + mark;
            product.Rating = totalRating / ++alreadyRated;
            product.AlreadyRated = alreadyRated;
            Context.Goods.Update(product);
            await Context.SaveChangesAsync();
            await Cache.SetStringAsync(product.Article.ToString(), JsonConvert.SerializeObject(product), CacheOptions);
        }

        public async Task<Product> GetProduct(int article)
        {
            Product? product;
            string? productString = await Cache.GetStringAsync(article.ToString());
            if (productString != null)
            {
                product = JsonConvert.DeserializeObject<Product>(productString);
                await Cache.RemoveAsync(article.ToString());
            }
            else
                product = await Context.Goods.FirstOrDefaultAsync(p => p.Article == article);
            if (product == null || !product.Visible)
                throw new DoesNotExistException(typeof(Product));
            await Cache.SetStringAsync(product.Article.ToString(), JsonConvert.SerializeObject(product), CacheOptions);
            return product;
        }

        public async Task<List<Product>> GetVisibleGoodsList(int page, int pageSize, GoodsListOptionsModel listOptions)
        {
            IQueryable<Product> goodsQuerry = Context.Goods.Where(p => p.Visible);
            if (listOptions.TextInTitle != null)
                goodsQuerry = goodsQuerry.Where(p => EF.Functions.Like(p.Title, $"%{listOptions.TextInTitle}%"));
            switch(listOptions.Criterium)
            {
                case SortCriteria.Price:
                    goodsQuerry = (listOptions.IsAsc ? goodsQuerry.OrderBy(p => p.Price) : goodsQuerry.OrderByDescending(p => p.Price));
                    break;
                case SortCriteria.Rating:
                    goodsQuerry = (listOptions.IsAsc ? goodsQuerry.OrderBy(p => p.Rating) : goodsQuerry.OrderByDescending(p => p.Rating));
                    break;
                case SortCriteria.CookingTime:
                    goodsQuerry = (listOptions.IsAsc ? goodsQuerry.OrderBy(p => p.AverageCookingTime) : goodsQuerry.OrderByDescending(p => p.AverageCookingTime));
                    break;
            }
            int pagesCount = await goodsQuerry.CountAsync();
            if (pagesCount > 0)
                pagesCount = pagesCount / pageSize + ((pagesCount % pageSize) == 0 ? 0 : 1);
            else
                pagesCount = 1;
            if (page > pagesCount || page < 1)
                throw new InvalidPageException();
            return await goodsQuerry.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<List<Product>> GetInvisibleGoodsList(int page, int pageSize, string? textInTitle)
        {
            IQueryable<Product> goodsQuerry = Context.Goods.Where(p => !p.Visible);
            if (textInTitle != null)
                goodsQuerry = goodsQuerry.Where(p => EF.Functions.Like(p.Title, $"%{textInTitle}%"));
            int pagesCount = await goodsQuerry.CountAsync();
            if (pagesCount > 0)
                pagesCount = pagesCount / pageSize + ((pagesCount % pageSize) == 0 ? 0 : 1);
            else
                pagesCount = 1;
            if (page > pagesCount || page < 1)
                throw new InvalidPageException();
            return await goodsQuerry.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<string?> AttachImage(string imageName, int article)
        {
            Product? product;
            string? productString = await Cache.GetStringAsync(article.ToString());
            if (productString != null)
            {
                product = JsonConvert.DeserializeObject<Product>(productString);
                await Cache.RemoveAsync(article.ToString());
            }
            else
                product = await Context.Goods.FirstOrDefaultAsync(p => p.Article == article);
            if (product == null || !product.Visible)
                throw new DoesNotExistException(typeof(Product));
            string? oldImageName = product.ImageName;
            product.ImageName = imageName;
            Context.Goods.Update(product);
            await Context.SaveChangesAsync();
            await Cache.SetStringAsync(product.Article.ToString(), JsonConvert.SerializeObject(product), CacheOptions);
            return oldImageName;
        }

        public async Task<string?> DetachImage(int article)
        {
            Product? product;
            string? productString = await Cache.GetStringAsync(article.ToString());
            if (productString != null)
            {
                product = JsonConvert.DeserializeObject<Product>(productString);
                await Cache.RemoveAsync(article.ToString());
            }
            else
                product = await Context.Goods.FirstOrDefaultAsync(p => p.Article == article);
            if (product == null || !product.Visible)
                throw new DoesNotExistException(typeof(Product));
            string? oldImageName = product.ImageName;
            product.ImageName = null;
            Context.Goods.Update(product);
            await Context.SaveChangesAsync();
            await Cache.SetStringAsync(product.Article.ToString(), JsonConvert.SerializeObject(product), CacheOptions);
            return oldImageName;
        }
    }
}