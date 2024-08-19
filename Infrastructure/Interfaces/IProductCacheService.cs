using Domain.Models.Entities.SQLEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IProductCacheService
    {
        public Task<Product?> Get(int article);
        public Task Save(Product product);
        public Task Remove(int article);
        public Task Update(Product product);
    }
}
