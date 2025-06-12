using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoLibrary.Models;

namespace VideoLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : Controller
    {
        private readonly AppDbContext _context;
        public GenreController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetAllGenres()
        {
            return await _context.Genres.ToListAsync();
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Genre>> GetGenreByName(string name)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == name);
            if (genre == null)
            {
                return NotFound();
            }
            return genre; // Vracamo po imenu žanra
        }
    }
   }
