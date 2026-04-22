using AnilistClone.Models;

namespace AnilistClone.Services.Interfaces
{
    public interface IMediaService
    {
        public Task<Media> GetMedia(int id);

        public Task<IEnumerable<Media>> GetAllMedia(int currentPage);

        Task<IEnumerable<Media>> SearchMedia(string search);
    }
}
