using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoLibrary.Models;

namespace VideoLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectorController : Controller
    {
        private readonly AppDbContext _context;
        public DirectorController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Director>>> GetAllDirectors()
        {
            return await _context.Directors.ToListAsync();
        }

        [HttpGet ("{name}")]
        public async Task<ActionResult<Director>> GetDirectorByName(string name)
        {
            var director = await _context.Directors.FirstOrDefaultAsync(d => d.Name == name);
            if (director == null)
            {
                return NotFound();
            }
            return director;
        }
    }
}
