using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGoodsAdminService
    {
        Task AddProduct(ProductInputModel product);
        Task RemoveProduct(int article);
        Task EditPrice(int article, decimal price);
        Task Show(int article);
        Task Hide(int article);
        Task<List<ProductOutputModel>> GetInvisibleGoodsList(int page, int pageSize, string? textInTitle);
        Task AttachImage(string imageName, int article);
        Task DetachImage(int article);
    }
}