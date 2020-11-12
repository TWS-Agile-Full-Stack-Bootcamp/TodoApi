using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly ITodoRepository todoRepository;

        public TodoController(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return Ok(todoRepository.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTodo(long id)
        {
            Todo todoOptional = todoRepository.FindById(id);

            if (todoOptional == null)
            {
                return NotFound();
            }

            return Ok(todoOptional);
        }

        [HttpPost]
        public async Task<ActionResult> SaveTodo(Todo todo)
        {
            todo.Id = todoRepository.GetAll().Count + 1;

            todoRepository.Add(todo);

            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodo(long id)
        {
            Todo todoOptional = todoRepository.FindById(id);

            if (todoOptional != null)
            {
                todoRepository.Delete(todoOptional);
                return Ok();
            }

            return NotFound();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> UpdateTodo(long id, Todo newTodo)
        {
            Todo todoOptional = todoRepository.FindById(id);

            if (todoOptional == null)
            {
                return NotFound();
            }
            else if (newTodo.Title == null)
            {
                return BadRequest();
            }

            todoRepository.Delete(todoOptional);
            Todo mergedTodo = todoOptional.Merge(newTodo);
            todoRepository.Add(mergedTodo);

            return Ok(mergedTodo);
        }
    }
}
