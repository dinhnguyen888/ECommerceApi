using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApi.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("descriptionDetail")]
        public string DescriptionDetail { get; set; }

        [BsonElement("tag")]
        public string Tag { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("specification")]
        public string Specification { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("imageUrls")]
        public List<string> ImageUrls { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
