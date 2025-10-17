using AnilistClone.Models;

namespace AnilistClone.Services.Interfaces
{
    public interface IAnimeService
    {

        public Task<Show> GetShow(int id);

        public Task<IEnumerable<Show>> GetShows(int currentPage);

        Task<IEnumerable<Show>> SearchShows(string search);

    }
}
