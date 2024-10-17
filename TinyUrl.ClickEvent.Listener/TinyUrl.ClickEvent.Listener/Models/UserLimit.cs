using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TinyUrl.ClickEvent.Listener.Models
{
    public class UserLimit
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]
        public int UserId { get; set; }
        public int DailyCalls { get; set; }
    }
}
