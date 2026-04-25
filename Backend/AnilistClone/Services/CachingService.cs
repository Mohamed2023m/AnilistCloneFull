using AnilistClone.Models;
using AnilistClone.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace AnilistClone.Services
{
    public class CachingService : ICachingService
    {
        private readonly IMediaService _mediaService;
        private readonly IMemoryCache _cache;

        public CachingService(IMemoryCache cache, IMediaService mediaService)
        {
            _cache = cache;
            _mediaService = mediaService;
        }

        private async Task<T> GetOrSet<T>(string cacheKey, Func<Task<T>> fetch, TimeSpan ttl)
        {
            if (_cache.TryGetValue(cacheKey, out T cachedValue))
            {
                return cachedValue;
            }

            var data = await fetch();

            _cache.Set(cacheKey, data, ttl);

            return data;
        }

        public Task<Media> GetMedia(int id)
        {
            string cacheKey = $"Show_{id}";

            return GetOrSet(cacheKey, () => _mediaService.GetMedia(id), TimeSpan.FromHours(6));
        }

        public Task<IEnumerable<Media>> GetAllMedia(int currentPage)
        {
            string cacheKey = $"All_Trending_Shows_Page_{currentPage}";

            return GetOrSet(
                cacheKey,
                () => _mediaService.GetAllMedia(currentPage),
                TimeSpan.FromHours(1)
            );
        }

        public Task<IEnumerable<Media>> SearchMedia(string searchTerm)
        {
            string cacheKey = $"Shows_Search_{searchTerm.Replace(" ", "_").ToLower()}";

            return GetOrSet(
                cacheKey,
                () => _mediaService.SearchMedia(searchTerm),
                TimeSpan.FromMinutes(5)
            );
        }
    }
}
