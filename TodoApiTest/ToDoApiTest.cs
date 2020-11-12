using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using TodoApi;
using TodoApi.Models;
using Xunit;

namespace TodoApiTest
{
    public class ToDoApiTest : TestBase
    {
        public ToDoApiTest(CustomWebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_return_ok_and_todos_when_get_all_todos()
        {
            // given
            var mockIToDoRepository = new Mock<ITodoRepository>();
            List<Todo> todos = new List<Todo>()
            {
                new Todo(id: 1, title: "Mock ToDo", completed: false, order: 0),
            };
            mockIToDoRepository.Setup(m => m.GetAll()).Returns(todos);

            var client = Factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddScoped((serviceProvider) => { return mockIToDoRepository.Object; });
                    });
                }).CreateClient();

            // when
            var returnToDos = await client.GetAsync("/api/todo");

            // then
            var responseBody = await returnToDos.Content.ReadAsStringAsync();
            var actualTodos = JsonConvert.DeserializeObject<List<Todo>>(responseBody);

            Assert.Equal(System.Net.HttpStatusCode.OK, returnToDos.StatusCode);
            Assert.Equal(todos, actualTodos);
        }

        [Fact]
        public async Task Should_return_ok_and_todo_when_get_todo_successfully()
        {
            // given
            var id = 1;
            var mockIToDoRepository = new Mock<ITodoRepository>();
            Todo expectedTodo = new Todo(id: id, title: "Mock ToDo", completed: false, order: 0);
            mockIToDoRepository.Setup(m => m.FindById(1)).Returns(expectedTodo);
            HttpClient client = SetupRepositoryMock(mockIToDoRepository);

            // when
            var response = await client.GetAsync($"/api/todo/{id}");

            // then
            var responseBody = await response.Content.ReadAsStringAsync();
            var actualTodo = JsonConvert.DeserializeObject<Todo>(responseBody);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expectedTodo, actualTodo);
        }

        [Fact]
        public async Task Should_return_not_found_when_get_todo_given_specific_id_not_exist()
        {
            // given
            var id = 1;
            var mockIToDoRepository = new Mock<ITodoRepository>();
            HttpClient client = SetupRepositoryMock(mockIToDoRepository);

            // when
            var response = await client.GetAsync($"/api/todo/{id}");

            // then
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_return_created_and_todo_when_save_todo_successfully()
        {
            // given
            var mockIToDoRepository = new Mock<ITodoRepository>();
            HttpClient client = SetupRepositoryMock(mockIToDoRepository);
            mockIToDoRepository.Setup(m => m.GetAll()).Returns(new List<Todo>());

            Todo request = new Todo(title: "Mock ToDo", completed: false);
            string content = JsonConvert.SerializeObject(request);

            // when
            var response = await client.PostAsync($"/api/todo/", new StringContent(content, Encoding.UTF8, "application/json"));

            // then
            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

            Assert.Matches("/api/Todo/\\S+", response.Headers.Location.LocalPath);
            Todo expectedTodo = new Todo(id: 1, title: "Mock ToDo", completed: false, order: 0);
            var responseBody = await response.Content.ReadAsStringAsync();
            var actualTodo = JsonConvert.DeserializeObject<Todo>(responseBody);
            Assert.Equal(expectedTodo, actualTodo);
        }

        [Fact]
        public async Task Should_return_ok_when_delete_todo_successfully()
        {
            // given
            var id = 1;
            var mockIToDoRepository = new Mock<ITodoRepository>();
            HttpClient client = SetupRepositoryMock(mockIToDoRepository);
            Todo todo = new Todo(id: 1, title: "Mock ToDo", completed: false, order: 0);
            mockIToDoRepository.Setup(m => m.FindById(id)).Returns(todo);

            // when
            var response = await client.DeleteAsync($"/api/todo/{id}");

            // then
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Should_return_not_found_when_delete_todo_given_specific_id_not_exist()
        {
            // given
            var id = 1;
            var mockIToDoRepository = new Mock<ITodoRepository>();
            HttpClient client = SetupRepositoryMock(mockIToDoRepository);
            mockIToDoRepository.Setup(m => m.FindById(id)).Returns<Todo>(null);

            // when
            var response = await client.DeleteAsync($"/api/todo/{id}");

            // then
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_return_ok_and_todo_when_update_todo_successfully()
        {
            // given
            var id = 1;
            Todo currentTodo = new Todo(id: id, title: "Mock ToDo", completed: true, order: 0);
            var mockIToDoRepository = new Mock<ITodoRepository>();
            HttpClient client = SetupRepositoryMock(mockIToDoRepository);
            mockIToDoRepository.Setup(m => m.FindById(id)).Returns(currentTodo);

            Todo request = new Todo(title: "Mock ToDo2", completed: true);
            string json = JsonConvert.SerializeObject(request);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            // when
            var response = await client.PutAsync($"/api/todo/{id}", content);

            // then
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);

            Todo expectedTodo = new Todo(id: 1, title: "Mock ToDo2", completed: true, order: 0);
            var responseBody = await response.Content.ReadAsStringAsync();
            var actualTodo = JsonConvert.DeserializeObject<Todo>(responseBody);
            Assert.Equal(expectedTodo, actualTodo);
        }

        [Fact]
        public async Task Should_return_not_found_when_update_todo_given_specific_id_not_exist()
        {
            // given
            var id = 1;
            var mockIToDoRepository = new Mock<ITodoRepository>();
            HttpClient client = SetupRepositoryMock(mockIToDoRepository);
            mockIToDoRepository.Setup(m => m.FindById(id)).Returns<Todo>(null);

            Todo request = new Todo(title: "Mock ToDo2", completed: true);
            string json = JsonConvert.SerializeObject(request);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            // when
            var response = await client.PutAsync($"/api/todo/{id}", content);

            // then
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        private HttpClient SetupRepositoryMock(Mock<ITodoRepository> mockIToDoRepository)
        {
            return Factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped((serviceProvider) => { return mockIToDoRepository.Object; });
                });
            }).CreateClient();
        }
    }
}