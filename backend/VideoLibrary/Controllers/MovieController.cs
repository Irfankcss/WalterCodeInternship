using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoLibrary.DTOs;
using VideoLibrary.Models;

namespace VideoLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MovieController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/movie
        // Returns a list of all movies in the system
        // This is useful when displaying all movies in a table or gallery
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMovies()
        {
            return await _context.Movies.Where(m=> !m.IsDeleted).ToListAsync();
        }

        // GET: api/movie/5
        // Returns a single movie by ID
        // Used to load detailed info about a movie or prepare it for editing
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Movie>> GetMovieById(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Director)
                .Include(m => m.EditedBy)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound();

            return movie;
        }

        // POST: api/movie
        // Creates a new movie and links it to existing actors, genres, director and editor by their IDs
        [HttpPost]
        public async Task<ActionResult<Movie>> CreateMovie([FromBody] MovieCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movie = new Movie
            {
                Name = dto.Name,
                Year = dto.Year,
                ImdbId = dto.ImdbId,
                ImdbRating = dto.ImdbRating,
                Poster = dto.Poster,
                DirectorId = dto.DirectorId,
                EditedById = dto.EditedById
            };

            foreach (var actorId in dto.ActorIds)
            {
                _context.MovieHasActors.Add(new MovieHasActor
                {
                    MovieId = movie.Id,
                    ActorId = actorId,
                    MovieDirectorId = dto.DirectorId,
                    MainActor = false
                });
            }
            foreach (var genreId in dto.GenreIds)
            {
                _context.GenreHasMovies.Add(new GenreHasMovie
                {
                    MovieId = movie.Id,
                    GenreId = genreId
                });
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
        }


        // PUT: api/movie/5
        // Updates an existing movie with the specified ID
        // Used when editing movie details from the admin interface
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] MovieUpdateDto dto)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();

            movie.ImdbRating = dto.ImdbRating;
            movie.Poster = dto.Poster;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        // DELETE: api/movie/5
        // Deletes a movie by ID.
        // Soft delete is implemented by marking the movie as deleted
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();

            movie.IsDeleted = true;
            movie.DeleteDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
    }
}
