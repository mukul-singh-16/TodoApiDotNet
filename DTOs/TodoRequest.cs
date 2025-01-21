namespace TodoApp.DTOs
{
    public class TodoRequest
    {
        
        public required string Username { get; set; }
        public  string? Title { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
