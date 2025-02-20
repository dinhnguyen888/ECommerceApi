using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ECommerceApi.Models
{
    public class Comment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        // represents the page where the comment is made, this page can be a product page, a blog post page, etc.
        [BsonElement("pageId")]
        public string PageId { get; set; }
        [BsonElement("userId")]
        public string UserId { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }
        [BsonElement("replyTo")]
        public string? ReplyTo { get; set; }
    }
    

}
