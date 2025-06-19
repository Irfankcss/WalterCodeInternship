using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoLibrary.Models;

namespace VideoLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RentalController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/rental
        // Returns all rental records
        // Useful for displaying a history of all rentals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetAll()
        {
            return await _context.Rentals
                .Include(r => r.BorrowedBy)
                .Include(r => r.BorrowedTo)
                .Include(r => r.MovieCopy)
                .Where(r=>!r.IsDeleted).ToListAsync();
        }

        // GET: api/rental/5
        // Returns a single rental record by its ID
        // Used when displaying details of one rental
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Rental>> GetById(int id)
        {
            var rental = await _context.Rentals
                .Include(r => r.BorrowedBy)
                .Include(r => r.BorrowedTo)
                .Include(r => r.MovieCopy)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rental == null)
                return NotFound();

            return rental;
        }

        // POST: api/rental
        // Creates a new rental record
        // Used when a movie is borrowed by a user
        [HttpPost]
        public async Task<ActionResult<Rental>> Create([FromBody] Rental rental)
        {
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = rental.Id }, rental);
        }

        // PUT: api/rental/5
        // Updates an existing rental record
        // Can be used to extend return date or fix user input errors
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Rental updatedRental)
        {
            if (id != updatedRental.Id)
                return BadRequest("ID in URL does not match rental ID.");

            var exists = await _context.Rentals.AnyAsync(r => r.Id == id);
            if (!exists)
                return NotFound();

            _context.Entry(updatedRental).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/rental/5
        // Deletes a rental record by ID.
        // Soft delete is implemented by marking the rental as deleted
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
                return NotFound();

            rental.IsDeleted = true;
            rental.DeleteDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
    }
}
