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

    [HttpPost("get-show")]
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

            return StatusCode(500, "An error occurred");
        }
    }

    [HttpPost("search-animes")]
    public async Task<IActionResult> SearchShows([FromBody] string search)
    {
        try
        {
            _logger.LogInformation("SearchShows is called with the string {search}",search);
            var shows = await _animeService.SearchShows(search); 
            return Ok(shows);
        } catch (Exception ex) {

            _logger.LogError(ex,"Error retrieving search result with the string {search}", search);
            return StatusCode(500, "An error occurred");
        }
    }

    [HttpGet("get-shows")]
    public async Task<IActionResult> GetShows()
    {   try
        {
            _logger.LogInformation("GetShows is called when retrieving a list of shows");
            var shows = await _animeService.GetShows();
            return Ok(shows);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error retrieving a list of shows");

            return StatusCode(500, "An error occurred");
        }
    }
}

