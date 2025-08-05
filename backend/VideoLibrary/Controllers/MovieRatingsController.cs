using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoLibrary.DataTransferObjects.MovieRating;
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
        public async Task<ActionResult<IEnumerable<MovieRating>>> GetAll()
        {
            return await _context.MovieRatings.Where( m=> !m.IsDeleted ).ToListAsync();
        }

        // GET: api/movieratings/movie/5/user/3
        // Returns a specific rating given by a user for a movie
        // Useful for showing/editing the user's personal review
        [HttpGet("movie/{movieId:int}/user/{userId:int}")]
        public async Task<ActionResult<MovieRating>> GetByMovieAndUser(int movieId, int userId)
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
        public async Task<ActionResult<MovieRating>> Create([FromBody] MovieRatingCreateDto dto)
        {
            var rating = new MovieRating
            {
                MovieId = dto.MovieId,
                UserId = dto.UserId,
                Rating = dto.Rating,
                Comment = dto.Comment
            };

            _context.MovieRatings.Add(rating);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByMovieAndUser), new { movieId = dto.MovieId, userId = dto.UserId }, rating);
        }


        // PUT: api/movieratings/movie/5/user/3
        // Updates an existing rating and/or comment
        // Useful when a user edits their previous review
        [Authorize]
        [HttpPut("movie/{movieId:int}/user/{userId:int}")]
        public async Task<IActionResult> Update(int movieId, int userId, [FromBody] MovieRatingCreateDto updatedRating)
        {
            var rating = await _context.MovieRatings
                .FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);

            if (rating == null)
                return NotFound();

            rating.Rating = updatedRating.Rating;
            rating.Comment = updatedRating.Comment;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        // DELETE: api/movierating/{movieId}/{userId}
        // Deletes a movie rating by movie ID and user ID.
        // Soft delete is implemented by marking the rating as deleted
        [Authorize]
        [HttpDelete("{movieId:int}/{userId:int}")]
        public async Task<IActionResult> Delete(int movieId, int userId)
        {
            var rating = await _context.MovieRatings
                .FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);
            if (rating == null)
                return NotFound();

            rating.IsDeleted = true;
            rating.DeleteDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
    }
}
