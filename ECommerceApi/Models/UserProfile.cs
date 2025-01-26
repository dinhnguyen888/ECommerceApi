using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ECommerceApi.Models
{
    public class UserProfile
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("avatar")]
        public string? Avatar { get; set; }

        [BsonElement("birthday")]
        public DateTime? Birthday { get; set; }

        [BsonElement("address")]
        public List<string>? Address { get; set; }

        [BsonElement("gender")]
        public string? Gender { get; set; }

    }
}
