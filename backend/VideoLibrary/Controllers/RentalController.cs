using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoLibrary.DataTransferObjects.Rental;
using VideoLibrary.DTOs;
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
        public async Task<ActionResult<IEnumerable<RentalDto>>> GetAll()
        {
            var rentals = await _context.Rentals
                .Include(r => r.BorrowedBy)
                .Include(r => r.BorrowedTo)
                .Include(r => r.MovieCopy)
                    .ThenInclude(mc => mc.Movie)
                .Where(r => !r.IsDeleted)
                .Select(r => new RentalDto
                {
                    Id = r.Id,
                    Date = r.Date,
                    ReturnDate = r.ReturnDate,
                    BorrowedById = r.BorrowedById,
                    BorrowedByName = r.BorrowedBy.Name,
                    BorrowedToId = r.BorrowedToId,
                    BorrowedToName = r.BorrowedTo.Name,
                    MovieCopyId = r.MovieCopyId,
                    MovieId = r.MovieCopy.MovieId,
                    MovieTitle = r.MovieCopy.Movie.Name
                })
                .ToListAsync();

            return Ok(rentals);
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
        // Creates a rental using only ID references for users and movie copy
        [HttpPost]
        public async Task<ActionResult<Rental>> Create([FromBody] RentalCreateDto dto)
        {
            var now = DateTime.Now;
            var hasActive = await _context.Rentals
                .AnyAsync(r => r.MovieCopyId == dto.MovieCopyId
                               && !r.IsDeleted
                               && r.ReturnDate >= now);

            if (hasActive)
            {
                return Conflict("This copy is already rented and has not been returned yet.");
            }

            var rental = new Rental
            {
                Date = now,
                ReturnDate = dto.ReturnDate,
                BorrowedToId = dto.BorrowedToId,
                MovieCopyId = dto.MovieCopyId,
                BorrowedById = 1 
            };

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = rental.Id }, rental);
        }


        // PUT: api/rental/5
        // Updates an existing rental record
        // Can be used to extend return date or fix user input errors
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] RentalUpdateDto dto)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
                return NotFound();

            rental.Date = dto.Date;
            rental.ReturnDate = dto.ReturnDate;

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
