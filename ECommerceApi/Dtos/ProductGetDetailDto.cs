using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ECommerceApi.Dtos
{
    public class ProductGetDetailDto
    {
        [BsonElement("id")]
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

        [BsonElement("imageUrls")]
        public List<string> ImageUrls { get; set; } // Lấy toàn bộ danh sách hình ảnh

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("descriptionDetail")]
        public string DescriptionDetail { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }
    }

}
