using System;
using System.Collections.Generic;

namespace TodoApi.Models
{
    public interface ITodoRepository
    {
        public List<Todo> GetAll();
        Todo FindById(long id);
        void Add(Todo todo);
        void Delete(Todo todoOptional);
    }
}
