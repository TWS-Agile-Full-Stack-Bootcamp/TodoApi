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
        public async Task Should_get_todos_from_mock_repository()
        {
            var mockIToDoRepository = new Mock<ITodoRepository>();
            List<Todo> todos = new List<Todo>();
            todos.Add(new Todo()
            {
                Completed = false,
                Id = 1,
                Order = 1,
                Title = "Mock ToDo"
            });
            mockIToDoRepository.Setup(m => m.GetAll()).Returns(todos);
            // Arrange
            var client = Factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var serviceProvider = services.BuildServiceProvider();

                        services.AddScoped<ITodoRepository>((serviceProvider) => { return mockIToDoRepository.Object; });
                    });
                })
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            var returnToDos = await client.GetAsync("/api/todo");
            var responseBody = await returnToDos.Content.ReadAsStringAsync();
            var todo = JsonConvert.DeserializeObject<List<Todo>>(responseBody);

            Assert.False(todo.First().Completed);
            Assert.Equal("Mock ToDo",todo.First().Title);
        }
    }
}