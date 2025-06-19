using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoLibrary.Models;

namespace VideoLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ActorController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/actor
        // Returns all actors from the database
        // This is useful when we want to show a list of actors in the app
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> GetAllActors()
        {
            return await _context.Actors.Where(a=> !a.IsDeleted).ToListAsync();
        }

        // GET: api/actor/5
        // Returns a single actor by ID
        // Useful when showing actor details or loading data for editing
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Actor>> GetActorById(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
                return NotFound();

            return actor;
        }

        // POST: api/actor
        // Adds a new actor to the database
        // Used when creating a new actor from a form
        [HttpPost]
        public async Task<ActionResult<Actor>> CreateActor([FromBody] Actor actor)
        {
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetActorById), new { id = actor.Id }, actor);
        }

        // PUT: api/actor/5
        // Updates an existing actor by ID
        // This is used when a user edits an actor's information
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateActor(int id, [FromBody] Actor updatedActor)
        {
            if (id != updatedActor.Id)
                return BadRequest("ID mismatch");

            var exists = await _context.Actors.AnyAsync(a => a.Id == id);
            if (!exists)
                return NotFound();

            _context.Entry(updatedActor).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //DELETE: api/actor/5
        // Deletes an actor by ID.
        // Soft delete is implemented by marking the actor as deleted
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
                return NotFound();

            actor.IsDeleted = true;
            actor.DeleteDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
    }
}
