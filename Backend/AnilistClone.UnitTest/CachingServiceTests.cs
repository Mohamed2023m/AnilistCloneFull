using AnilistClone.Models;
using AnilistClone.Services;
using AnilistClone.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace AnilistClone.UnitTest
{
    public class CachingServiceTests
    {
        [Fact]
        public async Task GetMedia_CacheMiss_FetchesAndCaches()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());

            var mockMediaService = new Mock<IMediaService>();

            mockMediaService.Setup(x => x.GetMedia(1)).ReturnsAsync(new Media { Id = 1 });

            var cachingService = new CachingService(cache, mockMediaService.Object);

            var result = await cachingService.GetMedia(1);

            Assert.Equal(1, result.Id);

            mockMediaService.Verify(x => x.GetMedia(1), Times.Once);

            Assert.True(cache.TryGetValue("Show_1", out var cached));
            Assert.Equal(1, ((Media)cached).Id);
        }

        [Fact]
        public async Task GetMedia_CacheHit_DoesNotFetch()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());

            cache.Set("Show_1", new Media { Id = 1 });

            var mockService = new Mock<IMediaService>();

            mockService.Setup(x => x.GetMedia(1)).ReturnsAsync(new Media { Id = 2 });
            var service = new CachingService(cache, mockService.Object);

            var result = await service.GetMedia(1);

            Assert.Equal(1, result.Id);
            mockService.Verify(x => x.GetMedia(1), Times.Never);
            Assert.True(cache.TryGetValue("Show_1", out _));
        }

        [Fact]
        public async Task GetMedia_FetchFails_DoesNotCache()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());

            var mockService = new Mock<IMediaService>();

            mockService.Setup(x => x.GetMedia(1)).ThrowsAsync(new Exception("fail"));

            var service = new CachingService(cache, mockService.Object);

            await Assert.ThrowsAsync<Exception>(() => service.GetMedia(1));
            mockService.Verify(x => x.GetMedia(1), Times.AtLeastOnce);
            Assert.False(cache.TryGetValue("Show_1", out _));
        }
    }
}
