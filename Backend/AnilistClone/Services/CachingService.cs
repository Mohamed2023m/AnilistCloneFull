using AnilistClone.Models;
using AnilistClone.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace AnilistClone.Services
{
    public class CachingService : ICachingService
    {
        private readonly IMediaService _animeService;
        private readonly IMemoryCache _cache;
        private readonly CachingWrapper _wrapper;

        public CachingService(
            IMemoryCache cache,
            IMediaService animeService,
            CachingWrapper wrapper
        )
        {
            _cache = cache;
            _animeService = animeService;
            _wrapper = wrapper;
        }

        public async Task<Media> GetMedia(int id)
        {
            string cacheKey = $"Show_{id}";

            return await _wrapper.GetMedia(cacheKey, () => _animeService.GetMedia(id));
        }

        public async Task<IEnumerable<Media>> GetAllMedia(int currentPage)
        {
            string cacheKey = $"All_Trending_Shows_Page_{currentPage}";

            return await _wrapper.GetAllMedia(
                cacheKey,
                () => _animeService.GetAllMedia(currentPage)
            );
        }

        public async Task<IEnumerable<Media>> SearchMedia(string searchTerm)
        {
            string cacheKey = $"Shows_Search_{searchTerm.Replace(" ", "_").ToLower()}";

            return await _wrapper.SearchMedia(
                cacheKey,
                () => _animeService.SearchMedia(searchTerm)
            );
        }
    }
}
