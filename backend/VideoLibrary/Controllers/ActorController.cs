using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoLibrary.Models;

namespace VideoLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActorController : Controller
    {
        private readonly AppDbContext _context;

        public ActorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> GetAllActors()
        {
            return await _context.Actors.ToListAsync();
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Actor>> GetActorByName(string name)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Name == name);
            if (actor == null)
            {
                return NotFound();
            }
            return actor;
        }
    }
}
