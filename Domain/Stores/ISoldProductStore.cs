using Domain.Models.Entities;
using Domain.Models.Entities.MongoDBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Stores
{
    public interface ISoldProductStore
    {
        public Task<List<int>> GetHotArticleList(int goodsCount);
        public Task AddSoldProduct(SoldProduct soldProduct);
    }
}
