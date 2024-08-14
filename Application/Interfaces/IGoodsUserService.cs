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
        Task<ProductOutputModel> GetProduct(int article);
        Task<List<ProductOutputModel>> GetVisibleGoodsList(int page, int pageSize, GoodsListOptionsModel listOptions);
        Task<List<ProductOutputModel>> GetHotGoodsList(int goodsCount);
        Task RateProduct(int article, int mark);
    }
}
