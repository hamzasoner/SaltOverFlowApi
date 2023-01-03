using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaltOverFlowApi.Models;

namespace SaltOverFlowApi.Controllers;

[Route("api/[controller]")]
public class SearchController : Controller
{
    private readonly AzureSqlDb _context;

    public SearchController(AzureSqlDb context)
    {
        _context = context;
    }

    [HttpGet("{queries}")]
    public async Task<ActionResult<List<Post>>> Get(string queries)
    {
        if (_context.Posts != null)
        {
            var posts = await _context.Posts.ToListAsync();
            var searchList = posts.Where(post => post.Tags.Contains(queries)).Select(match => match).ToList();

            if (searchList.Count == 0)
            {
                return NotFound();
            }

            return Ok(searchList);
        }

        return Problem("Entity set 'AzureSqlDb.User'  is null.");
    }
}