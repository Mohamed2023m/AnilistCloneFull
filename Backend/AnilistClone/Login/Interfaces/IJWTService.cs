using AnilistClone.Login.Models;

namespace AnilistClone.Login.Interfaces
{
    public interface IJWTService
    {
        public void GenerateToken(User user, HttpContext context);
    }
}
