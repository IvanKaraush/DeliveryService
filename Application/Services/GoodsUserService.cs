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
        public GoodsUserService(IProductStore productStore, ISoldProductStore soldProductStore, IOptions<ServsOptions> options)
        {
            ProductStore = productStore;
            SoldProductStore = soldProductStore;
            Link = options.Value.GoodsImagesLinkTemplate;
        }
        private readonly IProductStore ProductStore;
        private readonly ISoldProductStore SoldProductStore;
        private readonly string Link;
        public async Task<List<ProductOutputModel>> GetHotGoodsList(int goodsCount)
        {
            List<int> articles = await SoldProductStore.GetHotArticleList(goodsCount);
            List<ProductOutputModel> pOMList = new List<ProductOutputModel>();
            foreach (int article in articles)
            {
                pOMList.Add(new ProductOutputModel(await ProductStore.GetProduct(article), Link));
            }
            return pOMList;
        }

        public async Task<ProductOutputModel> GetProduct(int article)
        {
            return new ProductOutputModel(await ProductStore.GetProduct(article), Link);
        }

        public async Task<List<ProductOutputModel>> GetVisibleGoodsList(int page, int pageSize, GoodsListOptionsModel listOptions)
        {
            List<Product> productList = await ProductStore.GetVisibleGoodsList(page, pageSize, listOptions);
            List<ProductOutputModel> pOMList = new List<ProductOutputModel>();
            foreach (Product product in productList)
            {
                pOMList.Add(new ProductOutputModel(product, Link));
            }
            return pOMList;
        }

        public async Task RateProduct(int article, int mark)
        {
            if (mark < 0 || mark > 5)
                throw new InvalidMarkException();
            await ProductStore.UpdateRating(article, mark);
        }
    }
}
