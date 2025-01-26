using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApi.Models
{
    public class Cart
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } 

        [BsonElement("userId")]
        public string UserId { get; set; } 

        [BsonElement("productName")]
        public string ProductName { get; set; }

        [BsonElement("productId")]
        public string ProductId { get; set; } 

        [BsonElement("productDescription")]
        public string ProductDescription { get; set; } 

        [BsonElement("price")]
        public decimal Price { get; set; }

        [BsonElement("addToCartAt")]
        public DateTime AddToCartAt { get; set; } 
    }
}
