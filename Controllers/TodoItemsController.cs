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
        public async Task<IActionResult> GetTodosByUsername(string Username)
        {
            var User = await _context.Users.FirstOrDefaultAsync(u => u.Username == Username);

            if (User == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var todos = await _context.TodoItems
                .Where(t => t.UserId == User.Id)
                .ToListAsync();

            return Ok(todos);
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


            if (todoRequest.Title == null)
            {
                return NotFound(new { message = "enter title also" });
            }


            var todo = new TodoItem
            {
                Title = todoRequest.Title,
                IsCompleted = todoRequest.IsCompleted,
                UserId = user.Id 
            };


            _context.TodoItems.Add(todo);
            await _context.SaveChangesAsync();

            // user.Todos.Add(todo);


            var todoResponse = new TodoResponse
            {
                Id = todo.Id,
                Title = todo.Title,
                IsCompleted = todo.IsCompleted,
                Username = user.Username
            };

            return CreatedAtAction(nameof(GetTodoById), new { id = todo.Id }, todoResponse);
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

            if (user == null|| user.Id!= todo.UserId)
            {
                return NotFound(new { message = "You are not an authorized user" });
            }

            if (todoRequest.Title == null)
            {
                return NotFound(new { message = "enter title also" });
            }

            todo.Title = todoRequest.Title;
            todo.IsCompleted = todoRequest.IsCompleted;

            _context.TodoItems.Update(todo);
            
            await _context.SaveChangesAsync();

            return Ok(todo);
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTodoStatus(int id, [FromBody] TodoRequest todoRequest)
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

            if (user.Id != todo.UserId)
            {
                return Unauthorized(new { message = "You are not authorized to update this todo" });
            }

            todo.IsCompleted = todoRequest.IsCompleted;

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
