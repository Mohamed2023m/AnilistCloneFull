using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AnilistClone.Login.Interfaces;
using AnilistClone.Login.Models;
using Microsoft.IdentityModel.Tokens;

namespace AnilistClone.Login
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _config;

        public JWTService(IConfiguration config)
        {
            _config = config;
        }

        public void GenerateToken(User user, HttpContext context)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, (user.UserType)),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );

            context.Response.Cookies.Append(
                "jwt_token",
                new JwtSecurityTokenHandler().WriteToken(token),
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Path = "/",
                    MaxAge = TimeSpan.FromMinutes(30),
                }
            );
        }
    }
}
