using Domain.Models.ApplicationModels;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models.Entities.MongoDBEntities
{
    public class Order
    {
        [BsonId]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Adress { get; set; }
        public Coordinates Coordinates { get; set; }
        public Dictionary<int, int> GoodsList { get; set; }//article|count
        public bool IsCooking { get; set; }
        public DateTime? TimeMarker { get; set; }
        public Card? PaymentCard { get; set; }//null==NULLичные
        public bool AreBonusesUsing { get; set; }
    }
}
