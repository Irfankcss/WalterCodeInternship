using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoLibrary.Models;

namespace VideoLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieRatingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MovieRatingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/movieratings
        // Returns all movie ratings
        // Useful when displaying global reviews or statistics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieRatings>>> GetAll()
        {
            return await _context.MovieRatings.ToListAsync();
        }

        // GET: api/movieratings/movie/5/user/3
        // Returns a specific rating given by a user for a movie
        // Useful for showing/editing the user's personal review
        [HttpGet("movie/{movieId:int}/user/{userId:int}")]
        public async Task<ActionResult<MovieRatings>> GetByMovieAndUser(int movieId, int userId)
        {
            var rating = await _context.MovieRatings
                .FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);

            if (rating == null)
                return NotFound();

            return rating;
        }

        // POST: api/movieratings
        // Adds a new rating for a movie by a user.
        // Called when a user submits a new review
        [HttpPost]
        public async Task<ActionResult<MovieRatings>> Create([FromBody] MovieRatings rating)
        {
            _context.MovieRatings.Add(rating);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByMovieAndUser), new { movieId = rating.MovieId, userId = rating.UserId }, rating);
        }

        // PUT: api/movieratings/movie/5/user/3
        // Updates an existing rating and/or comment
        // Useful when a user edits their previous review
        [HttpPut("movie/{movieId:int}/user/{userId:int}")]
        public async Task<IActionResult> Update(int movieId, int userId, [FromBody] MovieRatings updatedRating)
        {
            if (movieId != updatedRating.MovieId || userId != updatedRating.UserId)
                return BadRequest("ID mismatch.");

            var exists = await _context.MovieRatings.AnyAsync(r => r.MovieId == movieId && r.UserId == userId);
            if (!exists)
                return NotFound();

            _context.Entry(updatedRating).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/movierating/{movieId}/{userId}
        // Deletes a movie rating by movie ID and user ID.
        // Soft delete is implemented by marking the rating as deleted
        /*[HttpDelete("{movieId:int}/{userId:int}")]
        public async Task<IActionResult> Delete(int movieId, int userId)
        {
            var rating = await _context.MovieRatings
                .FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);
            if (rating == null)
                return NotFound();

            rating.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        */
    }
}
