using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ECommerceApi.Dtos
{
    public class ProductPostDto
    {


        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("tag")]
        public string Tag { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }
        [BsonElement("productUrl")]
        public string ProductUrl { get; set; }
    }
}
