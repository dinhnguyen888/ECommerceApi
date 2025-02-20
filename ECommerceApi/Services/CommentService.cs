using AutoMapper;
using ECommerceApi.Dtos;
using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        // Get all comments with pagination
        public async Task<IEnumerable<Comment>> GetAllCommentsAsync(int pageNumber, int pageSize)
        {
            var comments = await _comments
                .Find(_ => true)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return _mapper.Map<List<Comment>>(comments);
        }

        // Get comments by Page ID with pagination
        public async Task<IEnumerable<Comment>> GetCommentsByPageIdAsync(string pageId, int pageNumber, int pageSize)
        {
            var comments = await _comments
                .Find(comment => comment.PageId == pageId)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return _mapper.Map<List<Comment>>(comments);
        }

        // Post a new comment
        public async Task<Comment> PostCommentAsync(CommentPostDto commentDto)
        {
            var comment = _mapper.Map<Comment>(commentDto);
            comment.CreatedAt = DateTime.UtcNow;
            await _comments.InsertOneAsync(comment);
            return comment;
        }


        public async Task<Comment> ReplyCommentAsync(CommentReplyDto replyDto)
        {
            var reply = new Comment
            {
                Id = ObjectId.GenerateNewId().ToString(),
                PageId = replyDto.PageId,
                UserId = replyDto.UserId,
                UserName = replyDto.UserName,
                Content = replyDto.Content,
                CreatedAt = DateTime.UtcNow,
                Replies = new List<Comment>() 
            };

            // using Push to add a reply to the Replies array of a comment
            var update = Builders<Comment>.Update.Push(c => c.Replies, reply);
            var result = await _comments.UpdateOneAsync(c => c.Id == replyDto.CommentId, update);

            // Return reply if it is added successfully
            return result.IsAcknowledged && result.ModifiedCount > 0 ? reply : null;
        }


        // Delete a comment
        public async Task<bool> DeleteCommentAsync(string id)
        {
            // 1. Delete a comment having the given ID
            var deleteResult = await _comments.DeleteOneAsync(comment => comment.Id == id);

            if (deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0)
            {
                return true;
            }

            // 2. If comment is not deleted, try to delete a reply
            var filter = Builders<Comment>.Filter.ElemMatch(c => c.Replies, r => r.Id == id);
            var update = Builders<Comment>.Update.PullFilter(c => c.Replies, r => r.Id == id);

            var updateResult = await _comments.UpdateManyAsync(filter, update);

            // 3. Return true if any reply is deleted
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }


    }
}
