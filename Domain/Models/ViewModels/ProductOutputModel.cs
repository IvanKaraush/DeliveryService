using Domain.Models.Entities.SQLEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.VievModels
{
    public class ProductOutputModel
    {
        public ProductOutputModel(Product product, string linkTemplate)
        {
            Article = product.Article;
            Price = product.Price;
            Title = product.Title;
            ImageLink = product.ImageName != null ? linkTemplate + product.ImageName : null;
            AverageCookingTime = product.AverageCookingTime;
            Rating = product.Rating;
        }
        public int Article { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public string? ImageLink { get; set; }
        public TimeOnly? AverageCookingTime { get; set; }
        public float? Rating { get; set; }
    }
}
