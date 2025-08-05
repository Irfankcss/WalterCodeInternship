using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using VideoLibrary.Models;

namespace VideoLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFavoriteMoviesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserFavoriteMoviesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFavoriteMovieResponseDto>>> GetAll()
        {
            var favorites = await _context.UserFavoriteMovies
                .Include(uf => uf.Movie)
                .Include(uf => uf.User)
                .Select(uf => new UserFavoriteMovieResponseDto
                {
                    UserId = uf.UserId,
                    UserName = uf.User.Username,
                    MovieId = uf.MovieId,
                    MovieTitle = uf.Movie.Name, 
                    MoviePoster = uf.Movie.Poster 
                })
                .ToListAsync();

            return Ok(favorites);
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<UserFavoriteMovieResponseDto>>> GetByUserId(int userId)
        {
            var favorites = await _context.UserFavoriteMovies
                .Where(uf => uf.UserId == userId)
                .Include(uf => uf.Movie)
                .Select(uf => new UserFavoriteMovieResponseDto
                {
                    UserId = uf.UserId,
                    UserName = uf.User.Username,
                    MovieId = uf.MovieId,
                    MovieTitle = uf.Movie.Name,
                    MoviePoster = uf.Movie.Poster
                })
                .ToListAsync();
            if (favorites.Count == 0)
                return NotFound("No favorite movies found for this user.");
            return Ok(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavorites([FromBody] UserFavoriteMovieDto dto)
        {
            var exists = await _context.UserFavoriteMovies.AnyAsync(uf =>
                uf.UserId == dto.UserId && uf.MovieId == dto.MovieId);

            if (exists)
                return BadRequest("Movie already in favorites.");

            var favorite = new UserFavoriteMovie
            {
                UserId = dto.UserId,
                MovieId = dto.MovieId
            };

            _context.UserFavoriteMovies.Add(favorite);
            await _context.SaveChangesAsync();

            return Ok(favorite);
        }


        [HttpDelete]
        public async Task<IActionResult> RemoveFromFavorites(int userId, int movieId)
        {
            var favorite = await _context.UserFavoriteMovies.FindAsync(userId, movieId);
            if (favorite == null)
                return NotFound();

            _context.UserFavoriteMovies.Remove(favorite);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
    public class UserFavoriteMovieDto
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
    }
    public class UserFavoriteMovieResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public string MoviePoster { get; set; }

    }


}
