using Microsoft.AspNetCore.Mvc;
using Movies.dto;
using Movies.Services.Interfaces;

namespace Movies.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class MovieEventBookingController: ControllerBase
{
    private readonly IMovieEventBookingService _movieEventBookingService;

    public MovieEventBookingController(IMovieEventBookingService movieEventBookingService)
    {
        _movieEventBookingService = movieEventBookingService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> BookForAMovieEvent(MovieEventBookingDto movieEventBookingDto)
    {
        return Created("", await _movieEventBookingService.CreateAMovieEventBooking(movieEventBookingDto));
    }

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllBookings()
    {
        return Ok(await _movieEventBookingService.GetAllBookings());
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBookingsById(int id)
    {
        return Ok(await _movieEventBookingService.GetBookingById(id));
    }
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBookingsByEmail([FromQuery] string emailAddress)
    {
        return Ok(await _movieEventBookingService.GetBookingsByEmail(emailAddress));
    }
}