using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { 
            Console.WriteLine("application db context constructor");
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
