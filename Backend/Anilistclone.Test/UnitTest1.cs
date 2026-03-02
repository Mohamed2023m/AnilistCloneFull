using AnilistClone.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Anilistclone.Test
{
    public class UnitTest1
    {



        [Fact]
        public void Test1()
        {

            //Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());

            var service = new CachingWrapper(cache);

            string cacheKey = $"Show_{1}";
            //Act



        }
    }
}