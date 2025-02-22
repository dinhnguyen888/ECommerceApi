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

        [BsonElement("feature")]
        public string Feature { get; set; }

        [BsonElement("technologyUsed")]
        public string TechnologyUsed { get; set; }

        [BsonElement("imageUrls")]
        public List<string> ImageUrls { get; set; }

        [BsonElement("postedDate")]
        public DateTime PostedDate { get; set; } = DateTime.UtcNow;
        [BsonElement("productUrl")]
        public string ProductUrl { get; set; } // for store an project or tool in google drive
    }

}
