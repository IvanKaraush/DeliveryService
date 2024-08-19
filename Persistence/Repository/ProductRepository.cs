using Domain.Models.ApplicationModels;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Persistence.Exceptions;
using Infrastructure;
using Infrastructure.Interfaces;

namespace Persistence.Repository
{
    public class ProductRepository : IProductStore
    {
        public ProductRepository(SQLContext context, IProductCacheService cacheService) 
        {
            _context = context;
            _cacheService = cacheService;
        }
        private readonly SQLContext _context;
        private readonly IProductCacheService _cacheService;
        public async Task<Product> GetProduct(int article)
        {
            Product? product = await _cacheService.Get(article);
            if (product == null)
                product = await _context.Goods.FirstOrDefaultAsync(p => p.Article == article);
            if (product == null || !product.Visible)
                throw new DoesNotExistException(typeof(Product));
            await _cacheService.Save(product);
            return product;
        }

        public async Task<List<Product>> GetVisibleGoodsList(int page, int pageSize, GoodsListOptionsModel listOptions)
        {
            IQueryable<Product> goodsQuerry = _context.Goods.Where(p => p.Visible);
            if (listOptions.TextInTitle != null)
                goodsQuerry = goodsQuerry.Where(p => EF.Functions.Like(p.Title, $"%{listOptions.TextInTitle}%"));
            switch (listOptions.Criterium)
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
            IQueryable<Product> goodsQuerry = _context.Goods.Where(p => !p.Visible);
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
        public async Task AddProduct(Product product)
        {
            await _context.Goods.AddAsync(product);
            await _context.SaveChangesAsync();
            await _cacheService.Save(product);
        }

        public async Task EditPrice(int article, decimal price)
        {
            Product? product = await _cacheService.Get(article);
            if (product == null)            
                product = await _context.Goods.FirstOrDefaultAsync(p => p.Article == article);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            product.Price = price;
            _context.Goods.Update(product);
            await _context.SaveChangesAsync();
            await _cacheService.Update(product);
        }

        public async Task Hide(int article)
        {
            Product? product = await _cacheService.Get(article);
            if (product == null)
                product = await _context.Goods.FirstOrDefaultAsync(p => p.Article == article);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            product.Visible = false;
            _context.Goods.Update(product);
            await _context.SaveChangesAsync();
            await _cacheService.Update(product);
        }

        public async Task<string?> RemoveProduct(int article)
        {
            Product? product = await _cacheService.Get(article);
            if (product == null)            
                product = await _context.Goods.FirstOrDefaultAsync(p => p.Article == article);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            _context.Goods.Remove(product);
            await _context.SaveChangesAsync();
            await _cacheService.Remove(article);
            return product.ImageName;
        }

        public async Task Show(int article)
        {
            Product? product = await _cacheService.Get(article);
            if (product == null)
                product = await _context.Goods.FirstOrDefaultAsync(p => p.Article == article);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            product.Visible = true;
            _context.Goods.Update(product);
            await _context.SaveChangesAsync();
            await _cacheService.Update(product);
        }

        public async Task UpdateCookingTime(int article, TimeOnly lastCookingTime)
        {
            Product? product = await _cacheService.Get(article);
            if (product == null)
                product = await _context.Goods.FirstOrDefaultAsync(p => p.Article == article);
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
            _context.Goods.Update(product);
            await _context.SaveChangesAsync();
            await _cacheService.Update(product);
        }

        public async Task UpdateRating(int article, int mark)
        {
            Product? product = await _cacheService.Get(article);
            if (product == null)
                product = await _context.Goods.FirstOrDefaultAsync(p => p.Article == article);
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
            _context.Goods.Update(product);
            await _context.SaveChangesAsync();
            await _cacheService.Update(product);
        }

        public async Task AttachImage(string imageName, int article)
        {
            Product? product = await _cacheService.Get(article);
            if (product == null)
                product = await _context.Goods.FirstOrDefaultAsync(p => p.Article == article);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            product.ImageName = imageName;
            _context.Goods.Update(product);
            await _context.SaveChangesAsync();
            await _cacheService.Update(product);
        }

        public async Task<string?> DetachImage(int article)
        {
            Product? product = await _cacheService.Get(article);
            if (product == null)
                product = await _context.Goods.FirstOrDefaultAsync(p => p.Article == article);
            if (product == null)
                throw new DoesNotExistException(typeof(Product));
            string? oldImageName = product.ImageName;
            product.ImageName = null;
            _context.Goods.Update(product);
            await _context.SaveChangesAsync();
            await _cacheService.Update(product);
            return oldImageName;
        }
    }
}