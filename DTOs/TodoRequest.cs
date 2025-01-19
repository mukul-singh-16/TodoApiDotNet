namespace TodoApp.DTOs
{
    public class TodoRequest
    {
        // The username of the user creating or updating the Todo
        public required string Username { get; set; }

        // The title of the Todo
        public required string Title { get; set; }

        // A description of the Todo (optional)
        public string? Description { get; set; }

        // Whether the Todo is completed (default: false)
        public bool IsCompleted { get; set; } = false;
    }
}
