using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Http;

namespace Domain.Stores
{
    public interface IProductStore
    {
        public Task<Product> GetProduct(int article);
        public Task<List<Product>> GetVisibleGoodsList(int page, int pageSize, GoodsListOptionsModel listOptions);
        public Task<List<Product>> GetInvisibleGoodsList(int page, int pageSize, string? textInTitle);
        public Task AddProduct(Product product);
        public Task<string?> RemoveProduct(int article);
        public Task EditPrice(int article, decimal price);
        public Task UpdateCookingTime(int article, TimeOnly lastCookingTime);
        public Task UpdateRating(int article, int mark);
        public Task Show(int article);
        public Task Hide(int article);
        public Task AttachImage(string imageName, int article);
        public Task<string?> DetachImage(int article);
    }
}
