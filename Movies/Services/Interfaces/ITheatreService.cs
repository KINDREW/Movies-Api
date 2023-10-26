using Movies.dto;
using Movies.Model;

namespace Movies.Services.Interfaces;

public interface ITheatreService
{
    Task<MessageResponseDTO> CreateTheatre(TheatreRequestDTO theatre);
    Task<Theatre> GetTheatreById(int id);
    Task<Theatre> UpdateTheatre(int id, TheatreRequestDTO theatreRequestDto);
    Task<MessageResponseDTO> DeletedTheatreById(int id);
    Task<IEnumerable<Theatre>> GetAllTheatres();
    Task<MessageResponseDTO> AddMovieToTheatre(int theatreId, AddMovieToTheatreRequestDto movieId);
    Task<MessageResponseDTO> AddBulkMovieToTheatre(int theatreId, List<int> movieIds);
    Task<IEnumerable<Movie>> GetAllMoviesInATheatre(int id);
    Task<MessageResponseDTO> DeleteMovieFromTheatre(int theatreId, int movieId);
}