using Domain.Models.ApplicationModels;
using Domain.Models.Entities.SQLEntities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ProductCacheService : IProductCacheService
    {
        public ProductCacheService(IDistributedCache cache, IOptions<RepositoryOptions> repositoryOptions)
        {
            _cache = cache;
            _cacheOptions = new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(repositoryOptions.Value.CacheExpirationMins) };
        }
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _cacheOptions;
        public async Task<Product?> Get(int article)
        {
            Product? product = null;
            string? productString = await _cache.GetStringAsync(article.ToString());
            if (productString != null)
            {
                product = JsonSerializer.Deserialize<Product>(productString);
            }
            return product;
        }

        public async Task Remove(int article)
        {
            string? productString = await _cache.GetStringAsync(article.ToString());
            if (productString != null)
                await _cache.RemoveAsync(article.ToString());
        }

        public async Task Save(Product product)
        {
            string? productString = await _cache.GetStringAsync(product.Article.ToString());
            if (productString == null)
                await _cache.SetStringAsync(product.Article.ToString(), JsonSerializer.Serialize(product), _cacheOptions);
        }

        public async Task Update(Product product)
        {
            await Remove(product.Article);
            await Save(product);
        }
    }
}
