using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoLibrary.DataTransferObjects.Actor;
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
        [HttpGet("GetByMovieID")]
        public async Task<ActionResult<IEnumerable<Actor>>> GetActorsByMovieId(int MovieId)
        {
            var actors = await _context.MovieHasActors
                .Where(ma => ma.MovieId == MovieId && !ma.Actor.IsDeleted)
                .Select(ma => ma.Actor)
                .ToListAsync();
            if (actors == null || !actors.Any())
                return NotFound();
            return Ok(actors);
        }

        // POST: api/actor
        // Adds a new actor to the database
        // Used when creating a new actor from a form
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Actor>> CreateActor([FromBody] CreateActorDto actor)
        {
            var newActor = new Actor
            {
                Name = actor.Name,
                Dob = actor.Dob,
                Bio = actor.Bio,
            };
            _context.Actors.Add(newActor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetActorById), new { id = newActor.Id }, newActor);
        }

        // PUT: api/actor?id=5
        // Updates an existing actor by ID
        // This is used when a user edits an actor's information
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActorUpdateDto dto)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
                return NotFound();

            actor.Name = dto.Name;
            actor.Dob = dto.Dob;
            actor.Bio = dto.Bio;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        //DELETE: api/actor/5
        // Deletes an actor by ID.
        // Soft delete is implemented by marking the actor as deleted
        [Authorize(Roles = "Admin")]
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
