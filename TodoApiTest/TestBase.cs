using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using TodoApi;
using Xunit;

namespace TodoApiTest
{
    public class TestBase : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public TestBase(CustomWebApplicationFactory<Startup> factory)
        {
            this.Factory = factory;
        }

        protected CustomWebApplicationFactory<Startup> Factory { get; }
    }
}