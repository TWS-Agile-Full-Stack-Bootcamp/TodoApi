﻿using System;
using System.Collections.Generic;
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
        public async System.Threading.Tasks.Task Should_return_ok_and_todos_when_get_all_todosAsync()
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
        public async System.Threading.Tasks.Task Should_return_ok_and_todo_when_get_todo_successfully()
        {
            // given
            var id = 1;
            Todo expectedTodo = new Todo(id: 1, title: "Mock ToDo", completed: false, order: 0);
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
    }
}
