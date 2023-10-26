using Movies.dto;
using Movies.Model;

namespace Movies.Services.Interfaces;

public interface IMovieEventBookingService
{
    Task<MessageResponseDTO> CreateAMovieEventBooking(MovieEventBookingDto movieEventBookingDto);
    Task<IEnumerable<MovieEventBooking>> GetAllBookings();
    Task<MovieEventBooking> GetBookingById(int id);
    Task<List<MovieEventBooking>> GetBookingsByEmail(string emailAddress);
}