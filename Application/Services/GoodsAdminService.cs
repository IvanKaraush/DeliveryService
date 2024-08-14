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
        public GoodsAdminService(IProductStore productStore, IAuditLogStore auditLogStore, IOptions<ServsOptions> options) 
        {
            ProductStore = productStore;
            ImageDirectory = Directory.GetCurrentDirectory() + "\\" + options.Value.GoodsImagesPath;
            AuditLogStore = auditLogStore;
            Link = options.Value.GoodsImagesLinkTemplate;
        }
        private readonly IAuditLogStore AuditLogStore;
        private readonly IProductStore ProductStore;
        private readonly string Link;
        private readonly string ImageDirectory;
        public async Task AddProduct(ProductInputModel productInputModel, Guid admin)
        {
            await ProductStore.AddProduct(productInputModel.ToProduct());
            await AuditLogStore.AddRecord(new AuditLogRecord(admin, "Добавлен артикул " + productInputModel.Article));
        }

        public async Task AttachImage(IFormFile file, int article, Guid admin)
        {
            if (!file.ContentType.Contains("image"))
                throw new InvalidFileFormatException();
            Product product = await ProductStore.GetProduct(article);
            if (product.ImageName != null)
            {
                File.Delete(ImageDirectory + "\\" + product.ImageName);
                await AuditLogStore.AddRecord(new AuditLogRecord(admin, "Удалено изображение у артикула " + article));
            }
            using (FileStream fS = new FileStream(ImageDirectory + "\\" + file.FileName, FileMode.Create))
            {
                await file.CopyToAsync(fS);
            }
            await ProductStore.AttachImage(file.FileName, article);
            await AuditLogStore.AddRecord(new AuditLogRecord(admin, "Добавлено изображение к артикулу " + article));
        }

        public async Task DetachImage(int article, Guid admin)
        {
            string? imgName = await ProductStore.DetachImage(article);
            if (imgName != null) 
            {
                File.Delete(ImageDirectory + "\\" + imgName);
            }
            await AuditLogStore.AddRecord(new AuditLogRecord(admin, "Удалено изображение у артикула " + article));
        }

        public async Task EditPrice(int article, decimal price, Guid admin)
        {
            await ProductStore.EditPrice(article, price);
            await AuditLogStore.AddRecord(new AuditLogRecord(admin, "Изменена цена у артикула " + article));
        }

        public async Task<List<ProductOutputModel>> GetInvisibleGoodsList(int page, int pageSize, string? textInTitle)
        {
            List<Product> productList = await ProductStore.GetInvisibleGoodsList(page, pageSize, textInTitle);
            List<ProductOutputModel> pOMList = new List<ProductOutputModel>();
            foreach (Product product in productList)
            {
                pOMList.Add(new ProductOutputModel(product, Link));
            }
            return pOMList;
        }

        public async Task Hide(int article, Guid admin)
        {
            await ProductStore.Hide(article);
            await AuditLogStore.AddRecord(new AuditLogRecord(admin, "Скрыт артикул " + article));
        }

        public async Task RemoveProduct(int article, Guid admin)
        {
            string? imgName = await ProductStore.RemoveProduct(article);
            if (imgName != null)
            {
                File.Delete(ImageDirectory + "\\" + imgName);
            }
            await AuditLogStore.AddRecord(new AuditLogRecord(admin, "Удален артикул " + article));
        }

        public async Task Show(int article, Guid admin)
        {
            await ProductStore.Show(article);
            await AuditLogStore.AddRecord(new AuditLogRecord(admin, "Показан артикул " + article));
        }
    }
}
