using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ECommerceApi.Models
{
    public class Banner
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("bannerName")]
        public string BannerName { get; set; }
        [BsonElement("bannerUrl")]
        public string BannerUrl { get; set; }
    }
}
