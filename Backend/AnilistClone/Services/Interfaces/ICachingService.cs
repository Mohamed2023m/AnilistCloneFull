using AnilistClone.Models;

namespace AnilistClone.Services.Interfaces
{
    public interface ICachingService
    {
        Task<Media> GetMedia(int id);

        Task<IEnumerable<Media>> GetAllMedia(int currentPage);

        Task<IEnumerable<Media>> SearchMedia(string searchTerm);
    }
}
