namespace ECommerceApi.Dtos
{
    public class CommentUpdateDto
    {
        public string Content { get; set; }
        public string? ReplyTo { get; set; }
    }
}
