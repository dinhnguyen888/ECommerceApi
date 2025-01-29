using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApi.Dtos
{
    public class ProductUpdateDto
    {
        [BsonElement("title")]
        public string? Title { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }
        [BsonElement("tag")]
        public string Tag { get; set; }

        [BsonElement("price")]
        public double? Price { get; set; }

        [BsonElement("category")]
        public string? Category { get; set; }

        [BsonElement("imageUrl")]
        public string? ImageUrl { get; set; }
        [BsonElement("productUrl")]
        public string ProductUrl { get; set; }
    }
}
