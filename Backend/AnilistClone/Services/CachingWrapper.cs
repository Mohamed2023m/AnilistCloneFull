using AnilistClone.Models;
using Microsoft.Extensions.Caching.Memory;

namespace AnilistClone.Services
{
    public class CachingWrapper
    {
        private readonly IMemoryCache _cache;

        public CachingWrapper(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetMedia<T>(string cacheKey, Func<Task<T>> fetch)
        {
            if (_cache.TryGetValue(cacheKey, out T cachedShow))
            {
                return cachedShow;
            }

            var data = await fetch();
            _cache.Set(cacheKey, data, TimeSpan.FromHours(6));
            return data;
        }

        public async Task<T> GetAllMedia<T>(string cacheKey, Func<Task<T>> fetch)
        {
            if (_cache.TryGetValue(cacheKey, out T cachedShow))
            {
                return cachedShow;
            }

            var data = await fetch();
            _cache.Set(cacheKey, data, TimeSpan.FromHours(1));
            return data;
        }

        public async Task<T> SearchMedia<T>(string cacheKey, Func<Task<T>> fetch)
        {
            if (_cache.TryGetValue(cacheKey, out T cachedShow))
            {
                return cachedShow;
            }

            var data = await fetch();
            _cache.Set(cacheKey, data, TimeSpan.FromMinutes(5));
            return data;
        }
    }
}
