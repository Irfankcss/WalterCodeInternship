using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();

            return movie;
        }

        // POST: api/movie
        // Adds a new movie to the database
        // Called when creating a new movie from the frontend
        [HttpPost]
        public async Task<ActionResult<Movie>> CreateMovie([FromBody] Movie movie)
        {
            if (movie == null)
                return BadRequest("Movie cannot be null.");

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
        }

        // PUT: api/movie/5
        // Updates an existing movie with the specified ID
        // Used when editing movie details from the admin interface
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] Movie updatedMovie)
        {
            if (id != updatedMovie.Id)
                return BadRequest("ID mismatch");

            var exists = await _context.Movies.AnyAsync(m => m.Id == id);
            if (!exists)
                return NotFound();

            _context.Entry(updatedMovie).State = EntityState.Modified;
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
