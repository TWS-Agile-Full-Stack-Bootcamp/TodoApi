using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApi.Controllers;
using TodoApi.Models;
using TodoApi.Services;
using Xunit;

namespace TodoApiTest.Controllers
{
    public class TodoControllerTest
    {
        [Fact]
        public async Task Should_return_ok_and_todos_when_get_all_todosAsync()
        {
            // given
            List<Todo> expectedTodos = new List<Todo>() { new Todo(id: 1, title: "Mock ToDo", completed: false, order: 0), };
            var mockService = new Mock<ITodoRepository>();
            mockService.Setup(service => service.GetAll())
                .Returns(expectedTodos);
            var todoController = new TodoController(mockService.Object);

            // when
            ActionResult actionResult = await todoController.GetAll().ConfigureAwait(false);

            // then
            Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(expectedTodos, (actionResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task Should_return_ok_and_todo_when_get_todo_successfully()
        {
            // given
            var id = 1;
            Todo expectedTodo = new Todo(id: id, title: "Mock ToDo", completed: false, order: 0);
            var mockService = new Mock<ITodoRepository>();
            mockService.Setup(service => service.FindById(id))
                .Returns(expectedTodo);
            var todoController = new TodoController(mockService.Object);

            // when
            ActionResult actionResult = await todoController.GetTodo(id).ConfigureAwait(false);

            // then
            Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(expectedTodo, (actionResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task Should_return_not_found_when_get_todo_given_specific_id_not_exist()
        {
            // given
            var id = 1;
            var mockService = new Mock<ITodoRepository>();
            mockService.Setup(service => service.FindById(id))
                .Returns<Todo>(null);
            var todoController = new TodoController(mockService.Object);

            // when
            ActionResult actionResult = await todoController.GetTodo(id).ConfigureAwait(false);

            // then
            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public async Task Should_return_created_and_todo_when_save_todo_successfully()
        {
            // given
            Todo requestTodo = new Todo(title: "Mock ToDo", completed: false);
            var mockService = new Mock<ITodoRepository>();
            mockService.Setup(service => service.GetAll())
                .Returns(new List<Todo>());
            var todoController = new TodoController(mockService.Object);

            // when
            ActionResult actionResult = await todoController.SaveTodo(requestTodo).ConfigureAwait(false);

            // then
            Assert.IsType<CreatedAtActionResult>(actionResult);
            // TODO test header location
        }

        [Fact]
        public async Task Should_return_ok_when_delete_todo_successfully()
        {
            // given
            var id = 1;
            Todo todo = new Todo(id: id, title: "Mock ToDo", completed: false, order: 0);
            var mockService = new Mock<ITodoRepository>();
            mockService.Setup(service => service.FindById(1))
                .Returns(todo);
            var todoController = new TodoController(mockService.Object);

            // when
            ActionResult actionResult = await todoController.DeleteTodo(id).ConfigureAwait(false);

            // then
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task Should_return_ok_and_todo_when_update_todo_successfully()
        {
            // given
            var id = 1;
            Todo currentTodo = new Todo(id: id, title: "Mock ToDo", completed: false, order: 0);
            var mockService = new Mock<ITodoRepository>();
            mockService.Setup(service => service.FindById(1))
                .Returns(currentTodo);
            var todoController = new TodoController(mockService.Object);

            Todo newTodo = new Todo(title: "Mock ToDo2", completed: true);

            // when
            ActionResult actionResult = await todoController.UpdateTodo(id, newTodo).ConfigureAwait(false);

            // then
            Todo expectedTodo = new Todo(id: id, title: "Mock ToDo2", completed: true, order: 0);
            Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(expectedTodo, (actionResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task Should_return_not_found_when_update_todo_given_specific_id_not_exist()
        {
            // given
            var id = 1;
            var mockService = new Mock<ITodoRepository>();
            mockService.Setup(service => service.FindById(1))
                .Returns<Todo>(null);
            var todoController = new TodoController(mockService.Object);

            Todo newTodo = new Todo(title: "Mock ToDo2", completed: true);

            // when
            ActionResult actionResult = await todoController.UpdateTodo(id, newTodo).ConfigureAwait(false);

            // then
            Assert.IsType<NotFoundResult>(actionResult);
        }
    }
}
