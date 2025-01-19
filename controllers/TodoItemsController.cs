using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Database;
using TodoApp.DTOs;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TodoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create Todo
        [HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] TodoRequest todoRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == todoRequest.Username);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var todo = new TodoItem
            {
                Title = todoRequest.Title,
                IsCompleted = todoRequest.IsCompleted,
                UserId = user.Id // Only set the UserId here
            };

            _context.TodoItems.Add(todo);
            await _context.SaveChangesAsync();

            var todoResponse = new TodoResponse
            {
                Id = todo.Id,
                Title = todo.Title,
                IsCompleted = todo.IsCompleted,
                Username = user.Username
            };

            return CreatedAtAction(nameof(GetTodoById), new { id = todo.Id }, todoResponse);
        }

        // Get All Todos
        [HttpGet]
        public async Task<IActionResult> GetAllTodos()
        {
            var todos = await _context.TodoItems.ToListAsync();
            return Ok(todos);
        }

        // Get Todo by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoById(int id)
        {
            var todo = await _context.TodoItems.FirstOrDefaultAsync(t => t.Id == id);

            if (todo == null)
            {
                return NotFound(new { message = "Todo not found" });
            }

            return Ok(todo);
        }

        // Get Todos by Username
        [HttpGet("user/{username}")]
        public async Task<IActionResult> GetTodosByUsername(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var todos = await _context.TodoItems
                .Where(t => t.UserId == user.Id)
                .ToListAsync();

            return Ok(todos);
        }

        // Update Todo
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody] TodoRequest todoRequest)
        {
            var todo = await _context.TodoItems.FindAsync(id);

            if (todo == null)
            {
                return NotFound(new { message = "Todo not found" });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == todoRequest.Username);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            todo.Title = todoRequest.Title;
            todo.IsCompleted = todoRequest.IsCompleted;
            todo.UserId = user.Id; // Only update UserId here

            _context.TodoItems.Update(todo);
            await _context.SaveChangesAsync();

            return Ok(todo);
        }

        // Delete Todo
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoById(int id)
        {
            var todo = await _context.TodoItems.FindAsync(id);

            if (todo == null)
            {
                return NotFound(new { message = "Todo not found" });
            }

            _context.TodoItems.Remove(todo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Todo deleted successfully" });
        }

        // Get Incomplete Todos by Username
        [HttpGet("user/{username}/incomplete")]
        public async Task<IActionResult> GetIncompleteTodos(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var incompleteTodos = await _context.TodoItems
                .Where(t => t.UserId == user.Id && !t.IsCompleted)
                .ToListAsync();

            return Ok(incompleteTodos);
        }

        // Get Completed Todos by Username
        [HttpGet("user/{username}/completed")]
        public async Task<IActionResult> GetCompletedTodos(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var completedTodos = await _context.TodoItems
                .Where(t => t.UserId == user.Id && t.IsCompleted)
                .ToListAsync();

            return Ok(completedTodos);
        }
    }
}
