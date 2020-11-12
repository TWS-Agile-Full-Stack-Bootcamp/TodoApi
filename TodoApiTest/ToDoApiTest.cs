using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
                        services.AddScoped<ITodoRepository>((serviceProvider) => { return mockIToDoRepository.Object; });
                    });
                })
                .CreateClient();

            // when
            var returnToDos = await client.GetAsync("/api/todo");

            // then
            var responseBody = await returnToDos.Content.ReadAsStringAsync();
            var actualTodos = JsonConvert.DeserializeObject<List<Todo>>(responseBody);

            Assert.Equal(System.Net.HttpStatusCode.OK, returnToDos.StatusCode);
            Assert.Equal(todos, actualTodos);
        }
    }
}