using Domain.Models.Entities;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Stores;
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
        public SoldProductRepository(IMongoContext context)
        {
            Context = context;
        }
        private readonly IMongoContext Context;

        public async void AddSoldProduct(SoldProduct soldProduct)
        {
            await Context.SoldGoods.InsertOneAsync(soldProduct);
        }

        public async Task<List<int>> GetHotArticleList(int goodsCount)
        {
            var querry = from SoldProduct in Context.SoldGoods.AsQueryable()
                      group SoldProduct by SoldProduct.Article into g
                      select new {Article = g.Key, Count =  g.Count()};
            return await querry.OrderByDescending(x => x.Count).Select(x=>x.Article).Take(goodsCount).ToListAsync();
        }
    }
}
