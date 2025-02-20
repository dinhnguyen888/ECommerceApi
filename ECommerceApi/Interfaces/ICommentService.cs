using ECommerceApi.Dtos;
using ECommerceApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ECommerceApi.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentGetDto>> GetCommentsByPageIdAsync(string pageId);
        Task<CommentGetDto> GetCommentByIdAsync(string id);
        Task<Comment> CreateCommentAsync(CommentPostDto comment);
        Task<Comment> UpdateCommentAsync(string id, CommentUpdateDto comment);
        Task<bool> DeleteCommentAsync(string id);
    }
}
