using System;
namespace TodoApi.Models
{
    public class Todo
    {
        public Todo()
        {
        }

        public Todo(string title, bool completed)
        {
            Title = title;
            Completed = completed;
        }

        public Todo(int id, string title, bool completed, int order)
        {
            Id = id;
            Title = title;
            Completed = completed;
            Order = order;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
        public int Order { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Todo todo &&
                   Id == todo.Id &&
                   Title == todo.Title &&
                   Completed == todo.Completed &&
                   Order == todo.Order;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title, Completed, Order);
        }

        public Todo Merge(Todo newTodo)
        {
            return new Todo(this.Id,
                NonNull(newTodo.Title, Title),
                NonNull(newTodo.Completed, Completed),
                NonNull(newTodo.Order, Order));
        }

        private T NonNull<T>(T value, T defaultValue)
        {
            return value == null ? defaultValue : value;
        }
    }
}
