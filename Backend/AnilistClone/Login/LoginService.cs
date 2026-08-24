using AnilistClone.AnilistClone.Data;
using AnilistClone.Login.DTOs.Requests;
using AnilistClone.Login.Interfaces;
using AnilistClone.Models;
using BCrypt.Net;

namespace AnilistClone.Login
{
    public class LoginService : ILoginService
    {
        private readonly AppDbContext _context;

        public LoginService(AppDbContext context)
        {
            _context = context;
        }

        public User AuthenticateUser(LoginRequest request)
        {
            var authenticatedUser = _context.Users.FirstOrDefault(u =>
                u.Username == request.Username
            );

            if (authenticatedUser == null)
            {
                return null;
            }

            bool isMatch = BCrypt.Net.BCrypt.Verify(request.Password, authenticatedUser.Password);

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
