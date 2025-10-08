using AnilistClone.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ShowController : ControllerBase
{
    private readonly IAnimeService _animeService;

    private readonly ILogger<ShowController> _logger;

    public ShowController(IAnimeService animeService, ILogger<ShowController> logger)
    {
        _animeService = animeService;
        _logger = logger;
    }

    [HttpPost("GetShow")]
    public async Task<IActionResult> GetShow([FromBody] int id)
    {

        _logger.LogInformation("GetShow is called with the id {ShowId}", id);

        try
        {
            var show = await _animeService.GetShow(id);
            return Ok(show);
        }
        catch(Exception ex)
        {
        _logger.LogError(ex, "Error retrieving show with the id {ShowId}", id);

            return StatusCode(500, "An error occured");
        }
    }

    [HttpPost("Search-Animes")]
    public async Task<IActionResult> SearchShows([FromBody] string search)
    {
       
        var shows = await _animeService.SearchShows(search); ;
        return Ok(shows);
    }

    [HttpGet("GetShows")]
    public async Task<IActionResult> GetShows()
    {
        var shows = await _animeService.GetShows();
        return Ok(shows);
    }
}

