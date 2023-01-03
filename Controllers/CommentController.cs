using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SaltOverFlowApi.Models;

namespace SaltOverFlowApi.Controllers
{
    [Route("/api/[controller]")]
    public class CommentController : Controller
    {
        private readonly AzureSqlDb _context;

        public CommentController(AzureSqlDb context)
        {
            _context = context;
        }

        [HttpPost("{postid:int}")]
        public async Task<IActionResult> Create(int postid, [FromBody] CommentRequest commentRequest)
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment()
                {
                    UserId = commentRequest.UserId,
                    Text = commentRequest.Text,
                    PostId = postid,
                };

                _context.Add(comment);
                await _context.SaveChangesAsync();
                return CreatedAtAction("Create", new { comment });
            }

            return BadRequest();
        }

        [HttpGet("{commentId:int}")]
        public async Task<IActionResult> Get(int? commentId)
        {
            if (commentId == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .FirstOrDefaultAsync(m => m.CommentId == commentId);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        [HttpGet("post/{postId:int}")]
        public async Task<ActionResult<List<Comment>>> Get(int postId)
        {
            if (_context.Comments != null)
            {
                var comments = await _context.Comments.ToListAsync();
                var filteredComments = comments
                    .Where(comment => comment.PostId == postId)
                    .Select(comments => comments)
                    .ToList();
                return Ok(filteredComments);
            }

            return Problem("Entity set 'AzureSqlDb.User'  is null.");
        }

        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<List<Comment>>> GetUserComments(int userId)
        {
            if (_context.Comments != null)
            {
                var comments = await _context.Comments.ToListAsync();
                var filteredComments = comments
                    .Where(comment => comment.UserId == userId)
                    .Select(comments => comments)
                    .ToList();
                return Ok(filteredComments);
            }

            return Problem("Entity set 'AzureSqlDb.User'  is null.");
        }

        [HttpGet]
        public async Task<ActionResult<List<Comment>>> GetAllComments()
        {
            if (_context.Comments != null)
            {
                var comments = await _context.Comments.ToListAsync();
                return Ok(comments);
            }

            return Problem("Entity set 'AzureSqlDb.User'  is null.");
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }

            await _context.SaveChangesAsync();
            return Ok("comment deleted");
        }

        [HttpPut("{commentId:int}")]
        public async Task<IActionResult> Edit(int commentId, [FromBody] CommentRequest commentRequest)
        {
            if (ModelState.IsValid)
            {
                var newComment = new Comment()
                {
                    CommentId = commentId,
                    PostId = commentRequest.PostId,
                    UserId = commentRequest.UserId,
                    Text = commentRequest.Text
                };
                _context.Update(newComment);
                await _context.SaveChangesAsync();
                return Ok(newComment);
            }

            return BadRequest();
        }
    }
}