using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.VievModels
{
    public class ProductInputModel
    {
        public int Article { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public TimeOnly? AverageCookingTime { get; set; }
        public float? Rating { get; set; }
    }
}
