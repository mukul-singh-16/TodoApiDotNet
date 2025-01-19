using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Database;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get All Users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            if (users == null || !users.Any())
            {
                return NotFound(new { message = "No users found in the database." });
            }

            return Ok(users);
        }

        // Add User
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            try
            {
                // Check if the username already exists
                if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                {
                    return Conflict(new { message = "Username already exists." });
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAllUsers), new { id = user.Id }, user);
            }
            catch (DbUpdateException ex)
            {
                // Handling unexpected errors during database updates
                throw;
            }
        }

        // Find User by Username
        [HttpGet("find/{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new
            {
                user.Id,
                user.Username
            });
        }

        // Update Username
        [HttpPut("update/{currentUsername}/{newUsername}")]
        public async Task<IActionResult> UpdateUsername(string currentUsername, string newUsername)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == currentUsername);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Check if the new username already exists
            if (await _context.Users.AnyAsync(u => u.Username == newUsername))
            {
                return Conflict(new { message = "Username already exists." });
            }

            user.Username = newUsername;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Username updated successfully", user });
        }

        // Delete User
        [HttpDelete("delete/{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully" });
        }
    }
}
