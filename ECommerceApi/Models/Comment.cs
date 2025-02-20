using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class Comment
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("pageId")]
    public string PageId { get; set; }

    [BsonElement("userId")]
    public string UserId { get; set; }

    [BsonElement("userName")]
    public string UserName { get; set; }

    [BsonElement("content")]
    public string Content { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; }

    [BsonElement("replies")]
    public List<Comment> Replies { get; set; } = new List<Comment>(); 
}
