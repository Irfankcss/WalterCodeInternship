using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoLibrary.Models;

namespace VideoLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DirectorController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/director
        // Returns all directors from the database 
        // Useful for listing in UI 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Director>>> GetAll()
        {
            return await _context.Directors.ToListAsync();
        }

        // GET: api/director/5
        // Returns a specific director by ID
        // Useful for displaying details or editing a director
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Director>> GetById(int id)
        {
            var director = await _context.Directors.FindAsync(id);
            if (director == null)
                return NotFound();

            return director;
        }

        // POST: api/director
        // Creates a new director entry in the database
        // Typically used when a user submits a form to add a new director
        [HttpPost]
        public async Task<ActionResult<Director>> Create([FromBody] Director director)
        {
            _context.Directors.Add(director);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = director.Id }, director);
        }

        // PUT: api/director/5
        // Updates an existing director by ID
        // Used when a user edits a director's information
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Director updatedDirector)
        {
            if (id != updatedDirector.Id)
                return BadRequest("ID u URL-u i objektu se ne poklapaju.");

            var exists = await _context.Directors.AnyAsync(d => d.Id == id);
            if (!exists)
                return NotFound();

            _context.Entry(updatedDirector).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/director/5
        // Deletes a director by ID.
        // Soft delete is implemented by marking the director as deleted
        /* [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var director = await _context.Directors.FindAsync(id);
            if (director == null)
                return NotFound();

            director.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        */
    }
}
