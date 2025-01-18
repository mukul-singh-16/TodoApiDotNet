namespace TodoApp.Models{
    public class User{

        public int  Id {get;set;}

        public required string Username {get;set;}

        public  List<TodoItem> Todos {get; set;}
        
    }
}