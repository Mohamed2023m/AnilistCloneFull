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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMediaById(int id)
    {
        if (id < 0)
        {
            return BadRequest();
        }
        var show = await _cachingService.GetMedia(id);
        return Ok(show);
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchMedia([FromBody] SearchRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var shows = await _cachingService.SearchMedia(request.searchTerm);
        return Ok(shows);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMedia([FromQuery] int currentPage)
    {
        var media = await _cachingService.GetAllMedia(currentPage);

        var dto = media
            .Select(Media => new MediaDto
            {
                Id = Media.Id,
                Title = Media.Title,
                coverImage = Media.CoverImage,
            })
            .ToList();

        return Ok(dto);
    }
}
