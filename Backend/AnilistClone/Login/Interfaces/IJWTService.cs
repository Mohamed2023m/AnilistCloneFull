using AnilistClone.Models;

namespace AnilistClone.Login.Interfaces
{
    public interface IJWTService
    {
        public string GenerateToken(User user);
    }
}
