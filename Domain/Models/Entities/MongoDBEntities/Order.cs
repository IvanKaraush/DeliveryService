using Domain.Models.Entities.SQLEntities;
using Domain.Models.VievModels;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models.Entities.MongoDBEntities
{
    public class Order
    {
        [BsonId]
        public Guid Id { get; set; }
        [BsonElement]
        public Guid UserId { get; set; }
        [BsonElement]
        public string Adress { get; set; }
        [BsonElement]
        public Coordinates Coordinates { get; set; }
        [BsonElement]
        public Dictionary<int, int> GoodsList { get; set; }
        [BsonElement]
        public bool IsCooking { get; set; }
        [BsonElement]
        public DateTime? TimeMarker { get; set; }
        [BsonElement]
        public string? PaymentCard { get; set; }
        [BsonElement]
        public bool AreBonusesUsing { get; set; }
    }
}
