using AnilistClone.Models;
using AnilistClone.Services;
using AnilistClone.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anilistclone.Test
{
    public class ShowControllerTest
    {
        [Fact]
        public async Task GetShow_Should_return_200ok_onFetchAndCache()
        {
            
            //Arrange
          var mockCaching = new Mock<ICachingService>();
          var mock = new Mock<ILogger<ShowController>>();
          ILogger<ShowController> logger = mock.Object;

          ShowController controller = new ShowController(mockCaching.Object,logger);

            mockCaching.Setup(caching => caching.GetShow(It.IsAny<int>()))
                    .ReturnsAsync(new Show());

            //Act
            var result = await controller.GetShow(1);

            var okResult = result as OkObjectResult;

            //Assert

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);

        }

    }
}
