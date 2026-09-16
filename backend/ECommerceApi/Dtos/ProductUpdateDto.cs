using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApi.Dtos
{
    public class ProductUpdateDto
    {


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


        [BsonElement("productUrl")]
        public string ProductUrl { get; set; } // for store an project or tool in google 
    }

}
