using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Database;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Constructor to inject the ApplicationDbContext
        public TodoItemsController(ApplicationDbContext context)
        {
            _context = context;     
        }





        // GET: api/TodoItems
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todoItems = await _context.TodoItems.ToListAsync();
            return Ok(todoItems); // Return list of all TodoItems
        }




        // GET: api/TodoItems/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            // Find the TodoItem by ID
            var item = await _context.TodoItems.FindAsync(id);

            if (item == null) 
            {
                return NotFound(); // Return 404 if item is not found
            }

            return Ok(item); // Return the found item
        }




        // POST: api/TodoItems
        [HttpPost]
        public async Task<IActionResult> Create(TodoItem todoItem)
        {
            // Add the new TodoItem to the context and save changes
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            // Return 201 Created status with the location of the new TodoItem
            return CreatedAtAction(nameof(Get), new { id = todoItem.Id }, todoItem);
        }



        // PUT: api/TodoItems/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateTodoItem(int id, [FromBody] TodoItem updatedTodo)
        {
            // Find the existing TodoItem by ID
            var existingTodo = _context.TodoItems.FirstOrDefault(t => t.Id == id);

            if (existingTodo == null)
            {
                return NotFound(); // Return 404 if item not found
            }

            // Update the fields of the existing TodoItem
            existingTodo.Title = updatedTodo.Title;
            existingTodo.IsCompleted = updatedTodo.IsCompleted;

            // Save the changes to the database
            _context.SaveChanges();

            // Return 204 No Content to indicate successful update
            return NoContent();
        }


        

        // DELETE: api/TodoItems/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Find the TodoItem by ID
            var item = await _context.TodoItems.FindAsync(id);

            if (item == null) 
            {
                return NotFound(); // Return 404 if item not found
            }

            // Remove the TodoItem from the context and save changes
            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();

            // Return 204 No Content indicating successful deletion
            return NoContent();
        }
    }
}
