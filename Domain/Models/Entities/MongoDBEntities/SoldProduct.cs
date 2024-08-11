using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models.Entities.MongoDBEntities
{
    public class SoldProduct
    {
        public SoldProduct(int hours)
        {
            ExpireAt = DateTime.Now.AddHours(hours);
            Id = Guid.NewGuid();
        }
        [BsonId]
        public Guid Id { get; set; }
        public DateTime ExpireAt { get; set; }
        public int Article {  get; set; }
    }
}
