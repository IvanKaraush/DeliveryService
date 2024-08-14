using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Microsoft.AspNetCore.Http;
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
        Task AddProduct(ProductInputModel product, Guid admin);
        Task RemoveProduct(int article, Guid admin);
        Task EditPrice(int article, decimal price, Guid admin);
        Task Show(int article, Guid admin);
        Task Hide(int article, Guid admin);
        Task<List<ProductOutputModel>> GetInvisibleGoodsList(int page, int pageSize, string? textInTitle);
        Task AttachImage(IFormFile file, int article, Guid admin);
        Task DetachImage(int article, Guid admin);
    }
}