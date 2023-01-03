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
    public class PostController : Controller
    {
        private readonly AzureSqlDb _context;

        public PostController(AzureSqlDb context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostRequest postRequest)
        {
            if (ModelState.IsValid)
            {
                var post = new Post
                {
                    UserId = postRequest.UserId,
                    Title = postRequest.Title,
                    Content = postRequest.Content,
                    Tags = postRequest.Tags,
                    Image = postRequest.Image,
                    Vote = postRequest.Vote
                };

                _context.Add(post);
                await _context.SaveChangesAsync();
                return CreatedAtAction("Create", new { post });
            }

            return BadRequest("bad request from in api");
        }

        [HttpGet("{postId:int}")]
        public async Task<ActionResult> Get(int? postId)
        {
            if (postId == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.Include(c => c.Comments)
                .FirstOrDefaultAsync(m => m.PostId == postId);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult> GetUserPosts(int? userId)
        {
            if (userId == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.Include(c => c.Comments)
                .Where(p => p.UserId == userId).Select(post => post).ToListAsync();
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> Get()
        {
            if (_context.Posts != null)
            {
                var posts = await _context.Posts.Include(c => c.Comments).ToListAsync();
                return Ok(posts);
            }

            return Problem("Entity set 'AzureSqlDb.User'  is null.");
        }

        [HttpDelete("{postId:int}")]
        public async Task<ActionResult> Delete(int? postId)
        {
            if (postId == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(postId);
            var comments = await _context.Comments
                .Where(comment => comment.PostId == postId)
                .Select(comment => comment)
                .ToListAsync();

            if (comments != null)
            {
                foreach (var comment in comments)
                {
                    _context.Comments.Remove(comment);
                }
            }

            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return Ok("post deleted");
        }

        [HttpPut("{postId:int}")]
        public async Task<ActionResult> Edit(int postId, [FromBody] PostRequest postRequest)
        {
            if (ModelState.IsValid)
            {
                var newPost = new Post()
                {
                    PostId = postId,
                    UserId = postRequest.UserId,
                    Title = postRequest.Title,
                    Content = postRequest.Content,
                    Tags = postRequest.Tags,
                    Image = postRequest.Image,
                    Vote = postRequest.Vote
                };
                _context.Update(newPost);
                await _context.SaveChangesAsync();
                return Ok(newPost);
            }

            return BadRequest();
        }

        [HttpGet("/search/{tags}")]
        public async Task<ActionResult<List<Post>>> Search(string tags)
        {
            var matchingPosts = new List<Post> { };
            var posts = await _context.Posts.ToListAsync();
            foreach (var post in posts)
            {
                if (post.Tags.Contains(tags))
                {
                    matchingPosts.Add(post);
                }
            }

            if (matchingPosts != null)
            {
                return Ok(matchingPosts);
            }

            return NotFound();
        }
    }
}