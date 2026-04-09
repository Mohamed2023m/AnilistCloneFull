using AnilistClone.DTOs.GetDTOs;
using AnilistClone.DTOs.PostDTOs;
using AnilistClone.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("[controller]")]
public class ShowController : ControllerBase
{
    private readonly ICachingService _cachingService;



    public ShowController(ICachingService cachingService)
    {
        _cachingService = cachingService;

    }

    [HttpGet("get-show")]
    public async Task<IActionResult> GetShow([FromQuery] int id)
    {

        if (id < 0)
        {
            return BadRequest();
        }
        var show = await _cachingService.GetShow(id);
        return Ok(show);

    }

    [HttpPost("search-animes")]
    public async Task<IActionResult> SearchShows([FromBody] SearchDTO request)
    {

        var shows = await _cachingService.SearchShows(request.searchTerm);
        return Ok(shows);


    }

    [HttpGet("get-shows")]
    public async Task<IActionResult> GetShows([FromQuery] int currentPage)
    {

        var shows = await _cachingService.GetShows(currentPage);

        var dto = shows
 .Select(show => new ShowDto
 {
     Id = show.Id,
     Title = show.Title,
     coverImage = show.CoverImage,


 })
 .ToList();

        return Ok(dto);
    }
}

