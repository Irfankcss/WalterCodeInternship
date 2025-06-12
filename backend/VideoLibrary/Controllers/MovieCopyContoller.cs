using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoLibrary.Models;

namespace VideoLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieCopyContoller : Controller
    {
        private readonly AppDbContext _context;
        public MovieCopyContoller(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieCopy>>> GetAllMovieCopies()
        {
            return await _context.MovieCopies.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieCopy>> GetMovieCopyById(int id)
        {
            var movieCopy = await _context.MovieCopies.FindAsync(id);
            if (movieCopy == null)
            {
                return NotFound();
            }
            return movieCopy;
        }
    }
}
