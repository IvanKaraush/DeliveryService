using Application.Exceptions;
using Application.Interfaces;
using Domain.Models.ApplicationModels;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GoodsUserService : IGoodsUserService
    {
        public GoodsUserService(IProductStore productStore, ISoldProductStore soldProductStore, IOptions<ServicesOptions> options)
        {
            _productStore = productStore;
            _soldProductStore = soldProductStore;
            _link = options.Value.GoodsImagesLinkTemplate;
        }
        private readonly IProductStore _productStore;
        private readonly ISoldProductStore _soldProductStore;
        private readonly string _link;
        public async Task<ProductOutputModel> GetProduct(int article)
        {
            return new ProductOutputModel(await _productStore.GetProduct(article), _link);
        }
        public async Task<List<ProductOutputModel>> GetHotGoodsList(int goodsCount)
        {
            List<int> articles = await _soldProductStore.GetHotArticleList(goodsCount);
            List<ProductOutputModel> pOMList = new List<ProductOutputModel>();
            foreach (int article in articles)
            {
                pOMList.Add(new ProductOutputModel(await _productStore.GetProduct(article), _link));
            }
            return pOMList;
        }
        public async Task<List<ProductOutputModel>> GetVisibleGoodsList(int page, int pageSize, GoodsListOptionsModel listOptions)
        {
            List<Product> productList = await _productStore.GetVisibleGoodsList(page, pageSize, listOptions);
            List<ProductOutputModel> pOMList = new List<ProductOutputModel>();
            foreach (Product product in productList)
            {
                pOMList.Add(new ProductOutputModel(product, _link));
            }
            return pOMList;
        }
        public async Task RateProduct(int article, int mark)
        {
            if (mark < 0 || mark > 5)
                throw new InvalidMarkException();
            await _productStore.UpdateRating(article, mark);
        }
    }
}
