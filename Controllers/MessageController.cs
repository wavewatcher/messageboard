#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using messageboard.Data;
using messageboard.Models;
using Microsoft.AspNetCore.Cors;

namespace messageboard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public MessageController(MyDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context        = context;
            _userManager    = userManager;
            _env            = env;
        }

        // GET: api/Message
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessage()
        {
            var messagesToReturn = _context.Message.Select(mes => new Message {
                Id = mes.Id,
                Content = mes.Content,
                Likes = mes.Likes,
                UserId = mes.UserId,
                CreationDate = mes.CreationDate,
                Image = mes.Image
            });

            return await messagesToReturn.ToListAsync();
        }

        // GET: api/Message/5
        [HttpGet("{id}")]
        [EnableCors("AnotherPolicy")]
        public async Task<ActionResult<Message>> GetMessage(int id)
        {
            var message = await _context.Message.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return message;
        }

        // PUT: api/Message/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMessage(int id, Message message)
        {
            if (id != message.Id)
            {
                return BadRequest();
            }

            _context.Entry(message).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
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

        private async Task<ApplicationUser> GetCurrentCuser() {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        // POST: api/Message
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessage(Message message)
        {
            // Altijd even de huidige datum en tijd bepalen voor het versturen van het bericht
            var currentDate = DateTime.Now;
            message.CreationDate = currentDate;
            _context.Message.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMessage", new { id = message.Id }, message);
        }

        // DELETE: api/Message/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }

            _context.Message.Remove(message);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MessageExists(int id)
        {
            return _context.Message.Any(e => e.Id == id);
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile() {
            // Method om eventuele bestanden bij een bericht op te slaan
            try {
                var httpRequest     = Request.Form;
                var postedFile      = httpRequest.Files[0];
                string filename     = postedFile.FileName;

                var physicalPath    = _env.ContentRootPath + "/Images/" + filename;

                using(var stream = new FileStream(physicalPath, FileMode.Create)) {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            } catch (Exception) {
                return new JsonResult("empty.png");
            }
        }
    }
}
