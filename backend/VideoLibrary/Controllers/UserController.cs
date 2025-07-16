using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VideoLibrary.DataTransferObjects.User;
using VideoLibrary.Dtos;
using VideoLibrary.DTOs;
using VideoLibrary.Helpers;
using VideoLibrary.Models;

namespace VideoLibrary.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;


        public UserController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/user
        // Returns a list of all users
        // Can be used by admin to manage users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _context.Users
                .Where(u => !u.IsDeleted)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Name = u.Name,
                    Dob = u.Dob,
                    Admin = u.Admin
                }).ToListAsync();

            return Ok(users);
        }


        // GET: api/user/5
        // Returns one user by their ID
        // Useful when showing a profile or editing data
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id && !u.IsDeleted)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    Name = u.Name,
                    Dob = u.Dob,
                    Admin = u.Admin
                }).FirstOrDefaultAsync();

            if (user == null)
                return NotFound();

            return Ok(user);
        }


        // POST: api/user
        // Creates a new user
        // Could be used for registration (without auth for now)
        [Authorize]
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
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.Username = dto.Username;
            user.Password = dto.Password;
            user.Email = dto.Email;
            user.Name = dto.Name;
            user.Dob = dto.Dob;
            user.Admin = dto.Admin;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        // DELETE: api/user/5
        // Deletes a user by ID.
        // Soft delete is implemented by marking the user as deleted
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            user.IsDeleted = true;
            user.DeleteDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto login)
        {
            var hashedPassword = PasswordHasher.Hash(login.Password);
            var user = _context.Users.FirstOrDefault(u => u.Username == login.Username && u.Password == hashedPassword);
            if (user == null || user.IsDeleted)
                return Unauthorized("Neispravni kredencijali.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Admin ? "Admin" : "User")
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return Ok(new { token = jwt });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest("Username već postoji");
            else if(await _context.Users.AnyAsync(u=> u.Email == dto.Email))
                return BadRequest("Email već postoji");


            var user = new User
                {
                    Username = dto.Username,
                    Password = PasswordHasher.Hash(dto.Password),
                    Email = dto.Email,  
                    Name = dto.Name,
                    Dob = dto.Dob,
                    Admin = false,
                    IsDeleted = false
                };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Registracija uspješna");
        }


    }
}
