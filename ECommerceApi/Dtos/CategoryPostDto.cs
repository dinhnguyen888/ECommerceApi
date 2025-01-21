using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApi.Dtos
{
    public class CategoryPostDto
    {
        [BsonElement("categoryName")]
        public string CategoryName { get; set; }
        [BsonElement("blockName")]
        public string BlockName { get; set; }
    }
}
