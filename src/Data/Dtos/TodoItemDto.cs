namespace ToDoApi.Data.Dtos
{
    public class TodoItemDto
    {
         public long id { get; set; }

        public string name { get; set; }

        public bool isComplete { get; set; }
    }
}