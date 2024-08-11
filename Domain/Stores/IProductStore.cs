using Domain.Models.Entities;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Http;

namespace Domain.Stores
{
    public interface IProductStore
    {
        void AddProduct(Product product);
        void RemoveProduct(int article);
        void EditPrice(int article, decimal price);
        void UpdateCookingTime(int article, TimeOnly lastCookingTime);
        void UpdateRating(int article, int mark);
        void Show(int article);
        void Hide(int article);
        Task<Product> GetProduct(int article);
        Task<List<Product>> GetVisibleGoodsList(int page, int pageSize, GoodsListOptionsModel listOptions);
        Task<List<Product>> GetInvisibleGoodsList(int page, int pageSize, string? textInTitle);
        Task<string?> AttachImage(string imageName, int article);
        Task<string?> DetachImage(int article);
    }
}
