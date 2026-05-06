using AnilistClone.Models;
using AnilistClone.Services;
using AnilistClone.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace AnilistClone.UnitTest
{
    public class CachingServiceTests
    {
        private class FakeMediaService : IMediaService
        {
            public bool WasCalled { get; private set; }

            public Func<Task<Media>> GetMediaFunc { get; set; }
            public Func<Task<IEnumerable<Media>>> GetAllMediaFunc { get; set; }
            public Func<Task<IEnumerable<Media>>> SearchMediaFunc { get; set; }

            public async Task<Media> GetMedia(int id)
            {
                WasCalled = true;
                return await GetMediaFunc();
            }

            public async Task<IEnumerable<Media>> GetAllMedia(int page)
            {
                WasCalled = true;
                return await GetAllMediaFunc();
            }

            public async Task<IEnumerable<Media>> SearchMedia(string term)
            {
                WasCalled = true;
                return await SearchMediaFunc();
            }
        }

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
