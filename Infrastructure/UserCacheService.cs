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
    public class UserCacheService:IUserCacheService
    {
        public UserCacheService(IDistributedCache cache, IOptions<RepositoryOptions> repositoryOptions)
        {
            _cache = cache;
            _cacheOptions = new DistributedCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(repositoryOptions.Value.CacheExpirationMins) };
        }
        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _cacheOptions;
        public async Task<User?> Get(Guid id)
        {
            User? user = null;
            string? userString = await _cache.GetStringAsync(id.ToString());
            if (userString != null)
            {
                user = JsonSerializer.Deserialize<User>(userString);
            }
            return user;
        }

        public async Task Remove(Guid id)
        {
            string? userString = await _cache.GetStringAsync(id.ToString());
            if (userString != null)
                await _cache.RemoveAsync(id.ToString());
        }

        public async Task Save(User user)
        {
            string? userString = await _cache.GetStringAsync(user.Id.ToString());
            if (userString == null)
                await _cache.SetStringAsync(user.Id.ToString(), JsonSerializer.Serialize(user), _cacheOptions);
        }

        public async Task Update(User user)
        {
            await Remove(user.Id);
            await Save(user);
        }
    }
}
