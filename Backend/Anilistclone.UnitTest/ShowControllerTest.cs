using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnilistClone.Models;
using AnilistClone.Services;
using AnilistClone.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Anilistclone.Test
{
    public class ShowControllerTest
    {
        [Fact]
        public async Task GetShow_Should_return_200ok_onFetchAndCache()
        {
            //Arrange
            var mockCaching = new Mock<ICachingService>();
            var mock = new Mock<ILogger<MediaController>>();
            ILogger<MediaController> logger = mock.Object;

            MediaController controller = new MediaController(mockCaching.Object);

            mockCaching
                .Setup(caching => caching.GetMedia(It.IsAny<int>()))
                .ReturnsAsync(new Media());

            //Act
            var result = await controller.GetMediaById(1);

            var okResult = result as OkObjectResult;

            //Assert

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task GetShow_ShouldReturnBadRequest_WhenIdIsNegativet()
        {
            //Arrange
            var mockCaching = new Mock<ICachingService>();
            var mock = new Mock<ILogger<MediaController>>();
            ILogger<MediaController> logger = mock.Object;
            MediaController controller = new MediaController(mockCaching.Object);

            //Act
            var result = await controller.GetMediaById(-1);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            mockCaching.Verify(caching => caching.GetMedia(It.IsAny<int>()), Times.Never);
        }
    }
}
