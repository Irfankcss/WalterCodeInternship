using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoLibrary.DTOs;
using VideoLibrary.Models;

namespace VideoLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieCopyController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MovieCopyController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/moviecopy
        // Returns all movie copies
        // Useful for listing available copies in stock or inventory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieCopy>>> GetAll()
        {
            return await _context.MovieCopies.Where(m=> !m.IsDeleted).ToListAsync();
        }

        // GET: api/moviecopy/by-movie/5
        // Returns all movie copies for the specified movie ID
        [HttpGet("by-movie/{movieId:int}")]
        public async Task<ActionResult<IEnumerable<MovieCopy>>> GetMovieCopiesByMovieId(int movieId)
        {
            var copies = await _context.MovieCopies
                .Where(mc => mc.MovieId == movieId)
                .ToListAsync();

            if (copies == null || !copies.Any())
                return NotFound();

            return Ok(copies);
        }

        // POST: api/moviecopy
        // Adds a new movie copy using only MovieId, not entire Movie object
        [HttpPost]
        public async Task<ActionResult<MovieCopy>> Create([FromBody] MovieCopyCreateDto dto)
        {
            var copy = new MovieCopy
            {
                MovieId = dto.MovieId,                                                                                                                                                                      
                SerialNumber = dto.SerialNumber,
                Description = dto.Description
            };

            _context.MovieCopies.Add(copy);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovieCopiesByMovieId), new { id = copy.Id }, copy);
        }

        // PUT: api/moviecopy/5
        // Updates an existing movie copy with new information
        // Used when modifying the copy's serial number or description
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] MovieCopy updatedCopy)
        {
            if (id != updatedCopy.Id)
                return BadRequest("ID mismatch");

            var exists = await _context.MovieCopies.AnyAsync(mc => mc.Id == id);
            if (!exists)
                return NotFound();

            _context.Entry(updatedCopy).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/moviecopy/5
        // Deletes a movie copy by ID.
        // Soft delete is implemented by marking the movie copy as deleted
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movieCopy = await _context.MovieCopies.FindAsync(id);
            if (movieCopy == null)
                return NotFound();

            movieCopy.IsDeleted = true;
            movieCopy.DeleteDate= DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
    }
}
