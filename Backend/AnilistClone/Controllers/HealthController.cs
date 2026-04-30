using Microsoft.AspNetCore.Mvc;

namespace AnilistClone.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [HttpHead]
        public IActionResult Check()
        {
            return Ok("ok");
        }
    }
}
