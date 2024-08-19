using Application.Exceptions;
using Application.Interfaces;
using Domain.Models.ApplicationModels;
using Domain.Models.Entities.MongoDBEntities;
using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using Domain.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GoodsAdminService : IGoodsAdminService
    {
        public GoodsAdminService(IProductStore productStore, IAuditLogStore auditLogStore, IOptions<ServicesOptions> options) 
        {
            _productStore = productStore;
            _imageDirectory = Directory.GetCurrentDirectory() + "\\" + options.Value.GoodsImagesPath;
            _auditLogStore = auditLogStore;
            _link = options.Value.GoodsImagesLinkTemplate;
        }
        private readonly IAuditLogStore _auditLogStore;
        private readonly IProductStore _productStore;
        private readonly string _link;
        private readonly string _imageDirectory;
        public async Task<List<ProductOutputModel>> GetInvisibleGoodsList(int page, int pageSize, string? textInTitle)
        {
            List<Product> productList = await _productStore.GetInvisibleGoodsList(page, pageSize, textInTitle);
            List<ProductOutputModel> pOMList = new List<ProductOutputModel>();
            foreach (Product product in productList)
            {
                pOMList.Add(new ProductOutputModel(product, _link));
            }
            return pOMList;
        }
        public async Task AddProduct(ProductInputModel productInputModel, Guid admin)
        {
            await _productStore.AddProduct(productInputModel.ToProduct());
            await _auditLogStore.AddRecord(new AuditLogRecord(admin, $"{AuditLogExpressions.ARTICLE_ADDED}{productInputModel.Article}"));
        }

        public async Task AttachImage(IFormFile file, int article, Guid admin)
        {
            if (!file.ContentType.Contains("image"))
                throw new InvalidFileFormatException();
            Product product = await _productStore.GetProduct(article);
            if (product.ImageName != null)
            {
                File.Delete(_imageDirectory + "\\" + product.ImageName);
                await _auditLogStore.AddRecord(new AuditLogRecord(admin, $"{AuditLogExpressions.IMAGE_REMOVED}{article}"));
            }
            using (FileStream fS = new FileStream(_imageDirectory + "\\" + file.FileName, FileMode.Create))
            {
                await file.CopyToAsync(fS);
            }
            await _productStore.AttachImage(file.FileName, article);
            await _auditLogStore.AddRecord(new AuditLogRecord(admin, $"{AuditLogExpressions.IMAGE_ADDED}{article}"));
        }

        public async Task DetachImage(int article, Guid admin)
        {
            string? imgName = await _productStore.DetachImage(article);
            if (imgName != null) 
            {
                File.Delete(_imageDirectory + "\\" + imgName);
            }
            await _auditLogStore.AddRecord(new AuditLogRecord(admin, $"{AuditLogExpressions.IMAGE_REMOVED}{article}"));
        }

        public async Task EditPrice(int article, decimal price, Guid admin)
        {
            await _productStore.EditPrice(article, price);
            await _auditLogStore.AddRecord(new AuditLogRecord(admin, $"{AuditLogExpressions.PRICE_CHANGED}{article}"));
        }

        public async Task HideProduct(int article, Guid admin)
        {
            await _productStore.Hide(article);
            await _auditLogStore.AddRecord(new AuditLogRecord(admin, $"{AuditLogExpressions.ARTICLE_HIDDEN}{article}"));
        }

        public async Task RemoveProduct(int article, Guid admin)
        {
            string? imgName = await _productStore.RemoveProduct(article);
            if (imgName != null)
            {
                File.Delete($"{_imageDirectory}\\{imgName}");
            }
            await _auditLogStore.AddRecord(new AuditLogRecord(admin, $"{AuditLogExpressions.ARTICLE_REMOVED}{article}"));
        }

        public async Task ShowProduct(int article, Guid admin)
        {
            await _productStore.Show(article);
            await _auditLogStore.AddRecord(new AuditLogRecord(admin, $"{AuditLogExpressions.ARTICLE_SHOWN}{article}"));
        }
    }
}
