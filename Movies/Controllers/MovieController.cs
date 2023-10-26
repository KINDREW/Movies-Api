using Microsoft.AspNetCore.Mvc;
using Movies.dto;
using Movies.Model;
using Movies.Services;

namespace Movies.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class MovieController: ControllerBase
{
    private readonly IMovieService _movieService;

    public MovieController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Movie))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult>GetMovieById(int id)
    {
        return Ok(await _movieService.GetMovieById(id));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<MessageResponseDTO>> CreateMovie(MovieRequestDTO movie)
    {
        return Created("",await _movieService.CreateMovie(movie));
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<Movie>> GetAllMoviesList()
    {
        return await _movieService.GetAllMoviesList();
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateMovie(int id, MovieRequestDTO requestDto)
    {
        return Ok(await _movieService.UpdateMovie(id, requestDto));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        return Ok(await _movieService.DeleteMovie(id));
    }
}
