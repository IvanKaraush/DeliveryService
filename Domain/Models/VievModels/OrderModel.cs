using Domain.Models.Entities.MongoDBEntities;
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
        public OrderModel(Order order)
        {
            Adress = order.Adress;
            Coordinates = order.Coordinates;
            GoodsList = order.GoodsList;
            PaymentCard = order.PaymentCard;
            AreBonusesUsing = order.AreBonusesUsing;
        }
        public string Adress { get; set; }
        public Coordinates Coordinates { get; set; }
        public Dictionary<int, int> GoodsList { get; set; }
        public string? PaymentCard { get; set; }
        public bool AreBonusesUsing { get; set; }
    }
}
