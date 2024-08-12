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
        Task AddSoldProduct(SoldProduct soldProduct);
        Task<List<int>> GetHotArticleList(int goodsCount);
    }
}
