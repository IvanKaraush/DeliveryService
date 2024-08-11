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
        public void AddSoldProduct(SoldProduct soldProduct);
        public Task<List<int>> GetHotArticleList(int goodsCount);
    }
}
