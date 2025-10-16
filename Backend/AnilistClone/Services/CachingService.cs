using AnilistClone.Models;
using AnilistClone.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace AnilistClone.Services
{
    public class CachingService
    {
        private readonly IAnimeService _animeService;
        private readonly IMemoryCache _cache;


        public CachingService(IMemoryCache cache, IAnimeService animeService)
        {

            _cache = cache;
            _animeService = animeService;

        }

        public async Task<Show> GetShow(int id)
        {

           string cacheKey = $"Show_{id}";

            if (_cache.TryGetValue(cacheKey, out Show cachedShow))
            {
                return cachedShow;
            }

            var myData = await _animeService.GetShow(id);

            _cache.Set(cacheKey, myData, TimeSpan.FromHours(6));

            return myData;

        }

        public async Task<IEnumerable<Show>> GetShows()
        {
            string cacheKey = $"All_Trending_Shows";


            if (_cache.TryGetValue(cacheKey, out IEnumerable<Show> cachedShows))
            {
                return cachedShows;
            }


            var myData = await _animeService.GetShows();

            _cache.Set(cacheKey, myData, TimeSpan.FromHours(1));

            return myData;
        }


        public async Task<IEnumerable<Show>> SearchShows(string searchTerm, int currentPage)
        {
            string cacheKey = $"Shows_Search_{searchTerm.Replace(" ", "_").ToLower()}_Page_{currentPage}";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<Show> cachedShows))
            {
                return cachedShows;
            }


            var myData = await _animeService.SearchShows(searchTerm, currentPage);

            _cache.Set(cacheKey, myData, TimeSpan.FromMinutes(5));

            return myData;
        }


    }
}
