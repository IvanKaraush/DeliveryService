using Domain.Models.ApplicationModels;
using Domain.Models.Entities.SQLEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.VievModels
{
    public class OrderModel
    {
        public string Adress { get; set; }
        public Coordinates Coordinates { get; set; }
        public Dictionary<int, int> GoodsList { get; set; }
        public string? PaymentCard { get; set; }
        public bool AreBonusesUsing { get; set; }
    }
}
