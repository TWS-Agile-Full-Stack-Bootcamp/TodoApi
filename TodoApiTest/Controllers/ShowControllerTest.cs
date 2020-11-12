using System;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApi.Controllers;
using TodoApi.Services;
using Xunit;

namespace TodoApiTest.Controllers
{
    public class ShowControllerTest
    {
        [Fact]
        public void Should_return_expected_message_when_GetAll_successfully()
        {
            // given
            string expectedMessage = "a beauty tag";
            var mockService = new Mock<ShowService>(null);
            mockService.Setup(service => service.GetShowLabel())
                .Returns(expectedMessage);
            var showController = new ShowController(mockService.Object);

            // when
            ObjectResult actionResult = showController.GetAll() as ObjectResult;

            // then
            Assert.Equal(200, actionResult.StatusCode);
            Assert.Equal(expectedMessage, actionResult.Value);
        }
    }
}
