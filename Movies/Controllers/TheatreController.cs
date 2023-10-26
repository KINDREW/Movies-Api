using Microsoft.AspNetCore.Mvc;
using Movies.dto;
using Movies.Model;
using Movies.Services;
using Movies.Services.Interfaces;

namespace Movies.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class TheatreController: ControllerBase
{
    private readonly ITheatreService _theatreService;

    public TheatreController(ITheatreService theatreService)
    {
        _theatreService = theatreService;
    }
    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<MessageResponseDTO>> CreateTheatre(TheatreRequestDTO theatre)
    {
        return Ok(await _theatreService.CreateTheatre(theatre));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Theatre>> GetTheatreById(int id)
    {
        return Ok(await _theatreService.GetTheatreById(id));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTheatre(int id, TheatreRequestDTO theatreRequestDto)
    {
        return Ok(await _theatreService.UpdateTheatre(id, theatreRequestDto));
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTheatres()
    {
        return Ok(await _theatreService.GetAllTheatres());
    }
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletedTheatreById(int id)
    {
        return Ok(await _theatreService.DeletedTheatreById(id));
    }

    [HttpPost("{id:int}/add-movie")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> AddMovieToTheatre(int id, [FromBody] AddMovieToTheatreRequestDto movieId)
    {
        return Ok(await _theatreService.AddMovieToTheatre(id, movieId));
    }

    [HttpPost("{id:int}/add-bulk-movie")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddBulkMoviesToTheatre(int id, List<int> movieIds)
    {
        return Ok(await _theatreService.AddBulkMovieToTheatre(id, movieIds));
    }

    [HttpGet("{id:int}/movies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllMoviesInATheatre(int id)
    {
        return Ok(await _theatreService.GetAllMoviesInATheatre(id));
    }

    [HttpDelete("{id:int}/movie/{movieId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteMovieFromTheatre(int id, int movieId)
    {
        return Ok(await _theatreService.DeleteMovieFromTheatre(id, movieId));
    }
}