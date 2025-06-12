using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoLibrary;

[ApiController]
[Route("api/[controller]")]
public class DatabaseTestController : ControllerBase
{
    private readonly AppDbContext _context;

    public DatabaseTestController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("test-connection")]
    public async Task<IActionResult> TestConnection()
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();
            if (canConnect)
            {
                return Ok("✅ Uspostavljena konekcija sa bazom podataka.");
            }
            else
            {
                return StatusCode(500, "❌ Neuspešna konekcija sa bazom.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"❌ Greška prilikom konekcije: {ex.Message}");
        }
    }
}
