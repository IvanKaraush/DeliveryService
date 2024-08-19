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
        public Task<List<ProductOutputModel>> GetInvisibleGoodsList(int page, int pageSize, string? textInTitle);
        public Task AddProduct(ProductInputModel product, Guid admin);
        public Task RemoveProduct(int article, Guid admin);
        public Task EditPrice(int article, decimal price, Guid admin);
        public Task ShowProduct(int article, Guid admin);
        public Task HideProduct(int article, Guid admin);
        public Task AttachImage(IFormFile file, int article, Guid admin);
        public Task DetachImage(int article, Guid admin);
    }
}