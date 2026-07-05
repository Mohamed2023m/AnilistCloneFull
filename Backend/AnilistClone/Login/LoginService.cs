using AnilistClone.Login.Interfaces;
using AnilistClone.Login.Models;
using BCrypt.Net;

namespace AnilistClone.Login
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository _repository;

        public LoginService(ILoginRepository respository)
        {
            respository = _repository;
        }

        public User AuthenticateUser(string username, string password)
        {
            var authenticatedUser = _repository.FetchHashedPassword(username);

            if (authenticatedUser == null)
            {
                return null;
            }

            bool isMatch = BCrypt.Net.BCrypt.Verify(password, authenticatedUser.Password);

            if (isMatch == true)
            {
                return authenticatedUser;
            }
            else
            {
                return null;
            }
        }
    }
}
