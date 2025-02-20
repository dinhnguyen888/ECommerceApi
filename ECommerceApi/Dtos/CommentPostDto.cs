namespace ECommerceApi.Dtos
{
    public class CommentPostDto
    {
        public string PageId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ReplyTo { get; set; }
    }
}
