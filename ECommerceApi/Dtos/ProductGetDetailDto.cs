using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ECommerceApi.Dtos
{
    public class ProductGetDetailDto
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("specification")]
        public string Specification { get; set; }
        [BsonElement("tag")]
        public string Tag { get; set; }

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
