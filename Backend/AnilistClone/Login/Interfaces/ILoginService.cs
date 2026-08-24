using AnilistClone.Login.DTOs.Requests;
using AnilistClone.Models;

namespace AnilistClone.Login.Interfaces
{
    public interface ILoginService
    {
        public User AuthenticateUser(LoginRequest request);
    }
}
