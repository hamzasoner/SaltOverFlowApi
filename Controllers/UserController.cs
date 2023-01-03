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
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly AzureSqlDb _context;

        public UserController(AzureSqlDb context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserRequest userRequest)
        {
            if (ModelState.IsValid)
            {
                var checkUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userRequest.Email);
                if (checkUser == null)
                {
                    var user = new User()
                    {
                        Name = userRequest.Name,
                        Email = userRequest.Email
                    };

                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("Create", new { user.UserId });
                }

                return Ok(new { checkUser.UserId });
            }

            return BadRequest("Error from webApi");
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.Include(c => c.Comments).Include(p => p.Posts)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            if (_context.Users != null)
            {
                var users = await _context.Users.Include(p => p.Posts).Include(c => c.Comments)
                    .ToListAsync();
                return Ok(users);
            }

            return Problem("Entity set 'AzureSqlDb.User'  is null.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id, [FromForm] UserRequest userRequest)
        {
            if (ModelState.IsValid)
            {
                var newUser = new User()
                {
                    UserId = id,
                    Name = userRequest.Name,
                    Email = userRequest.Email
                };
                _context.Update(newUser);
                await _context.SaveChangesAsync();
                return Ok(newUser);
            }

            return BadRequest();
        }
    }
}