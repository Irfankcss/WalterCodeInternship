using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoLibrary.Models;

namespace VideoLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieRatingController : Controller
    {
        private readonly AppDbContext _context;

        public MovieRatingController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet("{movieId}/{userId}")]  // Zbog kompozitnog kljuca koristimo dva parametra
        public async Task<IActionResult> GetRatingById(int movieId, int userId)
        {
            var rating = await _context.MovieRatings.FindAsync(movieId, userId);
            if (rating == null)
                return NotFound();

            return Ok(rating);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRating([FromBody] MovieRatings rating)
        {
            if (rating == null)
                return BadRequest("Rating cannot be null.");

            var existingRating = await _context.MovieRatings
                .FindAsync(rating.MovieId, rating.UserId); // Provera da li kljuc postoji

            if (existingRating != null)
                return Conflict("Rating for this movie by this user already exists.");

            _context.MovieRatings.Add(rating);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRatingById),
                new { movieId = rating.MovieId, userId = rating.UserId }, rating); // Vracamo oba kljuca jer je sastavljen od kompozitnog
        }
    }
}
