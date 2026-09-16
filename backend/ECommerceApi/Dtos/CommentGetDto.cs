public class CommentGetDto
{
    public string Id { get; set; }
    public string PageId { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<CommentGetDto> Replies { get; set; } = new List<CommentGetDto>(); 
}
