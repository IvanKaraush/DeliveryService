using Domain.Models.Entities.MongoDBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.VievModels
{
    public class OrderCreateModel
    {
        public string Adress { get; set; }
        public Coordinates Coordinates { get; set; }
        public Dictionary<int, int> GoodsList { get; set; }
        public string? PaymentCard { get; set; }
        public bool AreBonusesUsing { get; set; }
        public OrderModel ToOrderModel(Guid userId)
        {
            return new OrderModel() { Adress = Adress, AreBonusesUsing = AreBonusesUsing, Coordinates = Coordinates, GoodsList = GoodsList, PaymentCard = PaymentCard, UserId = userId };
        }
    }
}
