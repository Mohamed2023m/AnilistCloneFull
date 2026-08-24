using AnilistClone.Registration.DTOs.Requests;
using AnilistClone.Registration.DTOs.Responses;
using AnilistClone.Registration.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AnilistClone.Registration
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _service;

        public RegistrationController(IRegistrationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
        {
            var response = await _service.RegisterUser(request);

            return StatusCode(201, response);
        }
    }
}
