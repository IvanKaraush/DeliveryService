using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Http;

namespace Domain.Stores
{
    public interface IProductStore
    {
        Task AddProduct(Product product);
        Task<string?> RemoveProduct(int article);
        Task EditPrice(int article, decimal price);
        Task UpdateCookingTime(int article, TimeOnly lastCookingTime);
        Task UpdateRating(int article, int mark);
        Task Show(int article);
        Task Hide(int article);
        Task<Product> GetProduct(int article);
        Task<List<Product>> GetVisibleGoodsList(int page, int pageSize, GoodsListOptionsModel listOptions);
        Task<List<Product>> GetInvisibleGoodsList(int page, int pageSize, string? textInTitle);
        Task AttachImage(string imageName, int article);
        Task<string?> DetachImage(int article);
    }
}
