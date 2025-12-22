using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace backend.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("categoryName")]
        public string CategoryName { get; set; }

        [BsonElement("endpoint")]
        public string Endpoint { get; set; }
    }
}
