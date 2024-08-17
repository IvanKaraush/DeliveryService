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
        public OrderModel() { }
        public OrderModel(Order order)
        {
            Adress = order.Adress;
            Coordinates = order.Coordinates;
            GoodsList = order.GoodsList;
            PaymentCard = order.PaymentCard;
            AreBonusesUsing = order.AreBonusesUsing;
            UserId = order.UserId;
        }
        public Guid UserId { get; set; }
        public string Adress { get; set; }
        public Coordinates Coordinates { get; set; }
        public Dictionary<int, int> GoodsList { get; set; }
        public string? PaymentCard { get; set; }
        public bool AreBonusesUsing { get; set; }
        public Order ToOrder()
        {
            return new Order() { Adress = Adress, AreBonusesUsing = AreBonusesUsing, Coordinates = Coordinates, GoodsList = GoodsList, Id = Guid.NewGuid(), IsCooking = false, PaymentCard = PaymentCard, UserId = UserId };
        }
    }
}
