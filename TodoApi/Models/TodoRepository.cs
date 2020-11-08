using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoApi.Models
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ISet<Todo> todos = new HashSet<Todo>();

        public TodoRepository()
        {
            todos.Add(new Todo(1, "name", false, 1));
            todos.Add(new Todo(2, "name2", false, 2));
        }

        public Todo FindById(long id)
        {
            return todos.ToList().FirstOrDefault(todo => todo.Id == id);
        }

        public List<Todo> GetAll()
        {
            return todos.ToList();
        }

        public void Add(Todo todo)
        {
            if (todo.Id == 0)
            {
                todo.Id = todos.Count + 1;
                todo.Order = 1;
            }

            todos.Add(todo);
        }

        public void Delete(Todo todo)
        {
            todos.Remove(todo);
        }
    }
}
