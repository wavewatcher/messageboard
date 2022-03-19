#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using messageboard.Data;
using messageboard.Models;
using Microsoft.AspNetCore.Cors;

namespace messageboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public LikeController(MyDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Like
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Like>>> GetLike()
        {
            return await _context.Like.ToListAsync();
        }

        // GET: api/Like/5
        [HttpGet("{id}")]
        [EnableCors("AnotherPolicy")]
        public async Task<ActionResult<Like>> GetLike(int id)
        {
            var like = await _context.Like.FindAsync(id);

            if (like == null)
            {
                return NotFound();
            }
            return like;
        }

        // PUT: api/Like/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLike(int id, Like like)
        {
            if (id != like.id)
            {
                return BadRequest();
            }

            _context.Entry(like).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LikeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Like
        [HttpPost]
        public async Task<ActionResult<Like>> PostLike(Like like)
        {
            var currentDate = DateTime.Now;
            like.likeDate = currentDate;
            _context.Like.Add(like);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLike", new { id = like.id }, like);
        }

        // DELETE: api/Like/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLike(int id)
        {
            var like = await _context.Like.FindAsync(id);
            if (like == null)
            {
                return NotFound();
            }

            _context.Like.Remove(like);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LikeExists(int id)
        {
            return _context.Like.Any(e => e.id == id);
        }
    }
}
