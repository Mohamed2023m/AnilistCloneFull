using AnilistClone.Models;

namespace AnilistClone.Services.Interfaces
{
    public interface ICachingService
    {
        Task<Show> GetShow(int id);

        Task<IEnumerable<Show>> GetShows();

        Task<IEnumerable<Show>> SearchShows(string searchTerm, int currentPage);


    }
}
