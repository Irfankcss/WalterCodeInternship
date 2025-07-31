using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoLibrary.DataTransferObjects.Movie;
using VideoLibrary.DataTransferObjects.MovieCopy;
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
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetAllMovies()
        {
            var movies = await _context.Movies
                .Where(m => !m.IsDeleted)
                .Include(m => m.Director)
                .Include(m => m.EditedBy)
                .Include(m => m.MovieCopies)
                .ToListAsync();

            var movieDtos = movies.Select(m => new MovieDto
            {
                Id = m.Id,
                Name = m.Name,
                Year = m.Year,
                ImdbId = m.ImdbId,
                ImdbRating = m.ImdbRating,
                Poster = m.Poster,
                DirectorId = m.DirectorId,
                Director = m.Director,
                EditedById = m.EditedById,
                EditedByUsername = m.EditedBy.Username,
                MovieCopies = m.MovieCopies
                    .Where(c => !c.IsDeleted)
                    .Select(c => new MovieCopyDto
                    {
                        Id = c.Id,
                        SerialNumber = c.SerialNumber,
                        Description = c.Description
                    }).ToList()
            });

            return Ok(movieDtos);
        }


        // GET: api/movie/5
        // Returns a single movie by ID
        // Used to load detailed info about a movie or prepare it for editing
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieDto>> GetMovieById(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Director)
                .Include(m => m.EditedBy)
                .Include(m => m.MovieCopies)
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (movie == null)
                return NotFound();

            var movieDto = new MovieDto
            {
                Id = movie.Id,
                Name = movie.Name,
                Year = movie.Year,
                ImdbId = movie.ImdbId,
                ImdbRating = movie.ImdbRating,
                Poster = movie.Poster,
                DirectorId = movie.DirectorId,
                Director = movie.Director, 
                EditedById = movie.EditedById,
                EditedByUsername = movie.EditedBy.Username,
                MovieCopies = movie.MovieCopies
                    .Where(c => !c.IsDeleted)
                    .Select(c => new MovieCopyDto
                    {
                        Id = c.Id,
                        SerialNumber = c.SerialNumber,
                        Description = c.Description
                    }).ToList()
            };

            return Ok(movieDto);
        }


        //Zanrovi po filmu
        [HttpGet("{id:int}/genres")]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenresByMovie(int id)
        {
            var genres = await _context.GenreHasMovies
                .Where(gm => gm.MovieId == id && !gm.Genre.IsDeleted)
                .Include(gm => gm.Genre)
                .Select(gm => gm.Genre)
                .ToListAsync();

            return genres;
        }

        // POST: api/movie
        // Creates a new movie and links it to existing actors, genres, director and editor by their IDs
        [Authorize]
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

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            foreach (var actorId in dto.ActorIds)
            {
                _context.MovieHasActors.Add(new MovieHasActor
                {
                    MovieId = movie.Id,
                    ActorId = actorId,
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
        [Authorize]
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
        [Authorize]
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
