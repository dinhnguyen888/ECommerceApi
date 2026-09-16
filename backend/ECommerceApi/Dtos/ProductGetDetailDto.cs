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

        [BsonElement("feature")]
        public string Feature { get; set; }

        [BsonElement("technologyUsed")]
        public string TechnologyUsed { get; set; }

        [BsonElement("tag")]
        public string Tag { get; set; }

        [BsonElement("imageUrls")]
        public List<string> ImageUrls { get; set; } // Lấy toàn bộ danh sách hình ảnh

        [BsonElement("descriptionDetail")]
        public string DescriptionDetail { get; set; }

        [BsonElement("postedDate")]
        public DateTime PostedDate { get; set; }
    }

}
