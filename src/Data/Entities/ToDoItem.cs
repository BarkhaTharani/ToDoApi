namespace ToDoApi.Data.Entities {

    public class ToDoItem {

        public long id { get; set; }

        public string name { get; set; }

        public bool isComplete { get; set; }
    }
}