using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerceApi.Dtos;

namespace ECommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // Get all comments with pagination
        [HttpGet("all")]
        public async Task<IActionResult> GetAllComments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var comments = await _commentService.GetAllCommentsAsync(pageNumber, pageSize);
            return Ok(comments);
        }

        [HttpGet("page/{pageId}")]
        public async Task<IActionResult> GetCommentsByPageId(string pageId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var comments = await _commentService.GetCommentsByPageIdAsync(pageId, pageNumber, pageSize);
            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> PostComment([FromBody] CommentPostDto commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdComment = await _commentService.PostCommentAsync(commentDto);
            return CreatedAtAction(nameof(GetCommentsByPageId), new { pageId = createdComment.PageId }, createdComment);
        }

        [HttpPost("reply")]
        public async Task<IActionResult> ReplyComment([FromBody] CommentReplyDto replyDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reply = await _commentService.ReplyCommentAsync(replyDto);
            if (reply == null) return NotFound(new { Message = "Parent comment not found" });

            return Ok(reply);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(string id)
        {
            var result = await _commentService.DeleteCommentAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
