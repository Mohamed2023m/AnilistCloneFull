using AnilistClone.DTOs.GetDTOs;
using AnilistClone.DTOs.PostDTOs;
using AnilistClone.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class MediaController : ControllerBase
{
    private readonly ICachingService _cachingService;

    public MediaController(ICachingService cachingService)
    {
        _cachingService = cachingService;
    }

    [HttpGet("getMediaById")]
    public async Task<IActionResult> GetMediaById([FromQuery] int id)
    {
        if (id < 0)
        {
            return BadRequest();
        }
        var show = await _cachingService.GetMedia(id);
        return Ok(show);
    }

    [HttpPost("search-Media")]
    public async Task<IActionResult> SearchMedia([FromBody] SearchRequestDTO request)
    {
        var shows = await _cachingService.SearchMedia(request.searchTerm);
        return Ok(shows);
    }

    [HttpGet("GetAllMedia")]
    public async Task<IActionResult> GetAllMedia([FromQuery] int currentPage)
    {
        var shows = await _cachingService.GetAllMedia(currentPage);

        var dto = shows
            .Select(show => new MediaDto
            {
                Id = show.Id,
                Title = show.Title,
                coverImage = show.CoverImage,
            })
            .ToList();

        return Ok(dto);
    }
}
