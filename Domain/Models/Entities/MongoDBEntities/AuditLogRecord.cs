using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Models.Entities.MongoDBEntities
{
    public class AuditLogRecord
    {
        [BsonId]
        public DateTime Recorded { get; set; }
        public DateTime ExpireAt {  get; set; }
        public Guid AdminId { get; set; }
        public string Message { get; set; }
    }
}
