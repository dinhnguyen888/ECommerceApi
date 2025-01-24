using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace ECommerceApi.Dtos
{
    public class ProductGetDto
    {
        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("price")]
        public double Price { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("tag")]
        public string Tag { get; set; }

        [BsonElement("imageUrls")]
        [JsonIgnore]
        public List<string> ImageUrls { get; set; } 

        public string ImageUrl => ImageUrls?.FirstOrDefault(); 

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }
    }

}
