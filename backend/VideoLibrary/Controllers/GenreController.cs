using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoLibrary.Dtos;
using VideoLibrary.Models;

namespace VideoLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly AppDbContext _context;
        public GenreController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/genre
        // This returns all genres in the database
        // Useful when we want to show a list of all genres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetAllGenres()
        {
            return await _context.Genres.Where(g=>!g.IsDeleted).ToListAsync();
        }

        // GET: api/genre/5
        // This returns a single genre by its ID
        // We use this when we want to show genre details or edit a genre
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Genre>> GetGenreById(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
                return NotFound();
            return genre;
        }

        //Filmovi po zanru
        [HttpGet("{GenreId:int}/movies")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMoviesByGenre(int GenreId)
        {
            var movies = await _context.GenreHasMovies
                .Where(gm => gm.GenreId == GenreId && !gm.Movie.IsDeleted)
                .Include(gm => gm.Movie)
                    .ThenInclude(m => m.Director)
                .Include(gm => gm.Movie)
                    .ThenInclude(m => m.EditedBy)
                .Select(gm => gm.Movie)
                .ToListAsync();

            return movies;
        }

        // POST: api/genre
        // This adds a new genre to the database
        // It's used when we submit a form to create a new genre
        [HttpPost]
        public async Task<ActionResult<Genre>> CreateGenre([FromBody] Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGenreById), new { id = genre.Id }, genre);
        }

        // PUT: api/genre/5
        // This updates a genre with the given ID
        // Used when we want to change the name or description of a genre
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateGenre(int id, [FromBody] GenreUpdateDto dto)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
                return NotFound();

            genre.Name = dto.Name;
            genre.Description = dto.Description;

            await _context.SaveChangesAsync();
            return NoContent();
        }



        // DELETE: api/genre/5
        // Deletes a genre by ID.
        // Soft delete is implemented by marking the genre as deleted
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
                return NotFound();

            genre.IsDeleted = true;
            genre.DeleteDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
    }
}
