using AnilistClone.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Anilistclone.Test
{
    public class CachingWrapperTests
    {



        [Fact]
        public async Task GetShow_FetchesAndCaches_OnCacheMiss()
        {

            //Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());

            var service = new CachingWrapper(cache);

            string cacheKey = $"Show_{1}";

            bool wascalled = false;

           async Task<int> FakeFetchMethod()
            {

                wascalled = true;

                return 1;
            }
            //Act

            var result = await  service.GetShow(cacheKey, () =>  FakeFetchMethod());

            Assert.True(wascalled);
            Assert.Equal(1, result);
            Assert.True(cache.TryGetValue(cacheKey, out var value));
            Assert.Equal(1, (int)value);


        }


        [Fact]
        public async Task GetShow_CacheMiss_DoesNotCacheWhenFetchFails()
        {

            //Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());

            var service = new CachingWrapper(cache);

            string cacheKey = $"Show_{1}";

            bool wascalled = false;

            
            async Task<int> FakeFetchMethod()
            {
                wascalled = true;

                    throw new Exception("Method called");
                return 0;
            }
            //Act

            await Assert.ThrowsAnyAsync<Exception>(() => service.GetShow(cacheKey, () => FakeFetchMethod()));
            Assert.True(wascalled);
            Assert.False(cache.TryGetValue(cacheKey, out var value));

        }


        [Fact]
        public async Task GetShow_CacheHit_DoesNotFetch()
        {

            //Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());

            var service = new CachingWrapper(cache);

            string cacheKey = $"Show_{1}";

            bool wascalled = false;

            cache.Set(cacheKey, 1);


            async Task<int> FakeFetchMethod()
            {
                wascalled = true;

                return 0;
            }
            //Act
            var result = await service.GetShow(cacheKey, () => FakeFetchMethod());
            Assert.False(wascalled);
            Assert.True(cache.TryGetValue(cacheKey, out var value));
            Assert.Equal(1, result);

        }



    }
}