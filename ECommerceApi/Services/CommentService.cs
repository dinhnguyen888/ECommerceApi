using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ECommerceApi.Dtos;

namespace ECommerceApi.Services
{
    public class CommentService : ICommentService
    {
        private readonly IMongoCollection<Comment> _comments;
        private readonly IMapper _mapper;

        public CommentService(IConfiguration config, IMapper mapper)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDBConnection"));
            var database = client.GetDatabase("ECommerceDb");
            _comments = database.GetCollection<Comment>("Comments");
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentGetDto>> GetCommentsByPageIdAsync(string pageId)
        {
            var comments = await _comments.Find(comment => comment.PageId == pageId && comment.ReplyTo == null).ToListAsync();
            var commentDtos = _mapper.Map<List<CommentGetDto>>(comments);

            foreach (var commentDto in commentDtos)
            {
                commentDto.Replies = await GetRepliesAsync(commentDto.Id);
            }

            return commentDtos;
        }

        public async Task<CommentGetDto> GetCommentByIdAsync(string id)
        {
            var comment = await _comments.Find(comment => comment.Id == id).FirstOrDefaultAsync();
            if (comment == null) return null;

            var commentDto = _mapper.Map<CommentGetDto>(comment);
            commentDto.Replies = await GetRepliesAsync(commentDto.Id);

            return commentDto;
        }

        public async Task<Comment> CreateCommentAsync(CommentPostDto commentDto)
        {
            var comment = _mapper.Map<Comment>(commentDto);
            await _comments.InsertOneAsync(comment);
            return comment;
        }

        public async Task<Comment> UpdateCommentAsync(string id, CommentUpdateDto commentDto)
        {
            var comment = _mapper.Map<Comment>(commentDto);
            var result = await _comments.ReplaceOneAsync(c => c.Id == id, comment);
            return result.IsAcknowledged && result.ModifiedCount > 0 ? comment : null;
        }

        public async Task<bool> DeleteCommentAsync(string id)
        {
            var result = await _comments.DeleteOneAsync(comment => comment.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        private async Task<List<CommentGetDto>> GetRepliesAsync(string commentId)
        {
            var replies = await _comments.Find(comment => comment.ReplyTo == commentId).ToListAsync();
            return _mapper.Map<List<CommentGetDto>>(replies);
        }
    }
}
