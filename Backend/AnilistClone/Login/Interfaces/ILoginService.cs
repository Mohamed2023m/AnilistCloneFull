using AnilistClone.Login.Models;

namespace AnilistClone.Login.Interfaces
{
    public interface ILoginService
    {
        public User AuthenticateUser(string Username, string Password);
    }
}
