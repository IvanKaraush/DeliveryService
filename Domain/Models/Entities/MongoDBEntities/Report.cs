using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models.Entities.MongoDBEntities
{
    public class Report
    {
        [BsonId]
        public DateTime Received { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; }
    }
}
