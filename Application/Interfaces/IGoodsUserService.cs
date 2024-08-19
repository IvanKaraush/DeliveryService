using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGoodsUserService
    {
        public Task<ProductOutputModel> GetProduct(int article);
        public Task<List<ProductOutputModel>> GetVisibleGoodsList(int page, int pageSize, GoodsListOptionsModel listOptions);
        public Task<List<ProductOutputModel>> GetHotGoodsList(int goodsCount);
        public Task RateProduct(int article, int mark);
    }
}
