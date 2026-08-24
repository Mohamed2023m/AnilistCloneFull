using System.IdentityModel.Tokens.Jwt;
using AnilistClone.Login.DTOs.Requests;
using AnilistClone.Login.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnilistClone.Login
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        private readonly IJWTService _jwtService;

        public LoginController(ILoginService loginService, IJWTService jwtservice)
        {
            _loginService = loginService;
            _jwtService = jwtservice;
        }

        [HttpPost]
        public ActionResult Login(LoginRequest request)
        {
            var authenticatedUser = _loginService.AuthenticateUser(request);

            if (authenticatedUser == null)
            {
                return Unauthorized();
            }

            var token = _jwtService.GenerateToken(authenticatedUser);

            Response.Cookies.Append(
                "jwt_token",
                token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Path = "/",
                    MaxAge = TimeSpan.FromDays(1),
                }
            );

            return Ok();
        }
    }
}
