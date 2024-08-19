using Domain.Models.ApplicationModels;
using Domain.Models.Entities;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Stores;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repository
{
    public class SoldProductRepository : ISoldProductStore
    {
        public SoldProductRepository(IMongoContext context, IOptions<RepositoryOptions> options)
        {
            _context = context;
            _hotGoodsExpirationHours = options.Value.HotGoodsExpirationHours;
        }
        private readonly IMongoContext _context;
        private readonly int _hotGoodsExpirationHours;
        public async Task<List<int>> GetHotArticleList(int goodsCount)
        {
            var querry = from SoldProduct in _context.SoldGoods.AsQueryable()
                         group SoldProduct by SoldProduct.Article into g
                         select new { Article = g.Key, Count = g.Count() };
            return await querry.OrderByDescending(x => x.Count).Select(x => x.Article).Take(goodsCount).ToListAsync();
        }

        public async Task AddSoldProduct(SoldProduct soldProduct)
        {
            soldProduct.ExpireAt = DateTime.Now.AddHours(_hotGoodsExpirationHours);
            await _context.SoldGoods.InsertOneAsync(soldProduct);
        }
    }
}
