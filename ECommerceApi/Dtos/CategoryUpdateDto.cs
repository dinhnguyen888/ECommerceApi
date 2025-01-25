using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApi.Dtos
{
    public class CategoryUpdateDto
    {
        [BsonElement("categoryName")]
        public string CategoryName { get; set; }
        [BsonElement("blockName")]
        public string? BlockName { get; set; }
        [BsonElement("frontendEndpoint")]
        public string FrontendEndpoint { get; set; }
    }
}
