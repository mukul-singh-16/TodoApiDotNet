namespace TodoApp.DTOs
{
    public class TodoResponse
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public bool IsCompleted { get; set; }
        public required string Username { get; set; }
    }
}