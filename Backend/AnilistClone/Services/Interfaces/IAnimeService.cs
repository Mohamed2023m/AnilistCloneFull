using AnilistClone.Models;

namespace AnilistClone.Services.Interfaces
{
    public interface IAnimeService
    {

        public Task<Show> GetShow(int id);

        public Task<IEnumerable<Show>> GetShows();

        Task<IEnumerable<Show>> SearchShows(string search, int currentPage);

    }
}
