using Microsoft.AspNetCore.Mvc;
using Movies.dto;
using Movies.Model;
using Movies.Services;

namespace Movies.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class GenreController: ControllerBase
{
    private readonly IGenreService _genreService;

    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllGenres()
    {
        return  Ok(await _genreService.GetAllGenre());
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> AddGenre([FromBody] GenreRequestDTO genreRequestDto)
    {
        return Ok(await _genreService.AddGenre(genreRequestDto));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> UpdateGenre(int id, [FromBody] GenreRequestDTO genreRequestDto)
    {
        return Ok(await _genreService.UpdateGenre(id, genreRequestDto));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteGenre(int id)
    {
        return Ok(await _genreService.DeleteGenre(id));
    }

    [HttpPost("bulk")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> AddBulkGenre(GenreBulkRequest genreBulkRequest)
    {
        return Ok(await _genreService.AddBulkGenre(genreBulkRequest));
    }
}