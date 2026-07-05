using AnilistClone.Login.DTOs.Responses;
using AnilistClone.Login.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AnilistClone.Login
{
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
        public async Task<ActionResult> Login(string password, string username)
        {
            var authenticatedUser = _loginService.AuthenticateUser(username, password);

            _jwtService.GenerateToken(authenticatedUser, HttpContext);

            return Ok();
        }
    }
}
