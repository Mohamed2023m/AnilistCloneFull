using AnilistClone.Login.Models;

namespace AnilistClone.Login.Interfaces
{
    public interface ILoginRepository
    {
        public User FetchHashedPassword(string Username);
    }
}
