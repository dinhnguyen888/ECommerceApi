using ECommerceApi.Dtos;
using ECommerceApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceApi.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync(int pageNumber, int pageSize);
        Task<IEnumerable<Comment>> GetCommentsByPageIdAsync(string pageId, int pageNumber, int pageSize);
        Task<Comment> PostCommentAsync(CommentPostDto commentDto);
        Task<Comment> ReplyCommentAsync(CommentReplyDto replyDto);
        Task<bool> DeleteCommentAsync(string id);
    }
}
