using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            return await _context.MovieCopies.ToListAsync();
        }

        // GET: api/moviecopy/5
        // Returns a single movie copy by ID
        // Helpful when editing or inspecting specific copy details
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieCopy>> GetById(int id)
        {
            var copy = await _context.MovieCopies.FindAsync(id);
            if (copy == null)
                return NotFound();

            return copy;
        }

        // POST: api/moviecopy
        // Adds a new movie copy to the database
        // Called when registering a new physical or digital copy
        [HttpPost]
        public async Task<ActionResult<MovieCopy>> Create([FromBody] MovieCopy copy)
        {
            _context.MovieCopies.Add(copy);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = copy.Id }, copy);
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
        /*[HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movieCopy = await _context.MovieCopies.FindAsync(id);
            if (movieCopy == null)
                return NotFound();

            movieCopy.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        */
    }
}
