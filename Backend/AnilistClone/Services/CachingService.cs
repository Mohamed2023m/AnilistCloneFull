using AnilistClone.Models;
using AnilistClone.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace AnilistClone.Services
{
    public class CachingService : ICachingService
    {
        private readonly IAnimeService _animeService;
        private readonly IMemoryCache _cache;
        private readonly CachingWrapper _wrapper;


        public CachingService(IMemoryCache cache, IAnimeService animeService, CachingWrapper wrapper)
        {

            _cache = cache;
            _animeService = animeService;
            _wrapper = wrapper;
            

        }

        //public async Task<Show> GetShow(int id)
        //{

        //   string cacheKey = $"Show_{id}";

        //    if (_cache.TryGetValue(cacheKey, out Show cachedShow))
        //    {
        //        return cachedShow;
        //    }

        //    var myData = await _animeService.GetShow(id);

        //    _cache.Set(cacheKey, myData, TimeSpan.FromHours(6));

        //    return myData;

        //}


        public async Task<Show> GetShow(int id)
        {
            string cacheKey = $"Show_{id}";

            return  await _wrapper.GetShow(cacheKey,  () =>  _animeService.GetShow(id));
        }

        public async Task<IEnumerable<Show>> GetShows(int currentPage)
        {
            string cacheKey = $"All_Trending_Shows_Page_{currentPage}";


            return await _wrapper.GetShows(cacheKey, () => _animeService.GetShows(currentPage));
        }


        public async Task<IEnumerable<Show>> SearchShows(string searchTerm)
        {
            string cacheKey = $"Shows_Search_{searchTerm.Replace(" ", "_").ToLower()}";

      

         

            return await _wrapper.SearchShows(cacheKey, () => _animeService.SearchShows(searchTerm));
        }


    }
}
