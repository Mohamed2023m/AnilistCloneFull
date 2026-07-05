using AnilistClone.Login.Interfaces;
using AnilistClone.Login.Models;
using BCrypt.Net;

namespace AnilistClone.Login
{
    public class LoginRepository : ILoginRepository
    {
        public User FetchHashedPassword(string Username)
        {
            var UserwithHashedPW = new User
            {
                Username = "JohnPork",
                Password = "$2a$12$PuGpz3lWmDMWtUWcK31Sy.CsEaLPjSieA08MNxL9MQtp5y/.xqaM2",
                UserType = "User",
            };

            if (Username.Equals(UserwithHashedPW.Username))
            {
                return UserwithHashedPW;
            }

            return null;
        }
    }
}
