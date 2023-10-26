using Microsoft.AspNetCore.Mvc;
using Movies.dto;
using Movies.Services;
using Movies.Services.Interfaces;

namespace Movies.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class MovieShowController: ControllerBase
{
    private readonly IMovieShowService _movieShowService;

    public MovieShowController(IMovieShowService movieShowService)
    {
        _movieShowService = movieShowService;
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetMovieShowById(int id)
    {
        return Ok(await _movieShowService.GetMovieShowById(id));
    }
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllMovieShows()
    {
        return  Ok(await _movieShowService.GetAllMovieShows());
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateAMovieShow([FromBody] MovieShowRequestDto movieShowRequestDto)
    {
        return Created("",await _movieShowService.CreateMovieShow(movieShowRequestDto));
    }
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> UpdateMovieShow(int id, [FromBody] MovieShowRequestDto movieShowRequestDto)
    {
        return Ok(await _movieShowService.UpdateMovieShow(id,movieShowRequestDto));
    }
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteMovieShow(int id)
    {
        return Ok(await _movieShowService.DeleteMovieShow(id));
    }
}