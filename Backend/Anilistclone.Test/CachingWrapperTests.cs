using AnilistClone.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Anilistclone.Test
{
    public class CachingWrapperTests
    {



        [Fact]
        public async Task GetShow_Should_CallFetch_And_StoreValue_WhenCacheMisses()
        {

            //Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());

            var service = new CachingWrapper(cache);

            string cacheKey = $"Show_{1}";

            bool wascalled = false;

           async Task<int> FakeMethod()
            {

                wascalled = true;

                return 1;
            }
            //Act

            var result = await  service.GetShow(cacheKey, () =>  FakeMethod());

            Assert.True(wascalled);
            Assert.Equal(1, result);
            Assert.True(cache.TryGetValue(cacheKey, out var value));
            Assert.Equal(1, (int)value);


        }
    }
}