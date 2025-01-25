using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApi.Dtos
{
    public class BannerUpdateDto
    {
        [BsonElement("bannerName")]
        public string BannerName { get; set; }
        [BsonElement("bannerUrl")]
        public string BannerUrl { get; set; }
    }
}
