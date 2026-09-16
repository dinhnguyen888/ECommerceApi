using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApi.Dtos
{
    public class CategoryUpdateDto
    {
        [BsonElement("categoryName")]
        public string CategoryName { get; set; }
       
        [BsonElement("endpoint")]
        public string Endpoint { get; set; }
    }
}
