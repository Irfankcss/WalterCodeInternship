using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoLibrary.Models;

namespace VideoLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/user
        // Returns a list of all users
        // Can be used by admin to manage users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/user/5
        // Returns one user by their ID
        // Useful when showing a profile or editing data
        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return user;
        }

        // POST: api/user
        // Creates a new user
        // Could be used for registration (without auth for now)
        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        // PUT: api/user/5
        // Updates an existing user's data
        // Can be used by admin or for profile editing
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] User updatedUser)
        {
            if (id != updatedUser.Id)
                return BadRequest("ID in URL doesn't match ID in body.");

            var exists = await _context.Users.AnyAsync(u => u.Id == id);
            if (!exists)
                return NotFound();

            _context.Entry(updatedUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/user/5
        // Deletes a user by ID.
        // Soft delete is implemented by marking the user as deleted
        /*[HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        */
    }
}
