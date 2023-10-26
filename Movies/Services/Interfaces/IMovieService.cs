using Movies.dto;
using Movies.Model;
using Movies.Repository;

namespace Movies.Services;

public interface IMovieService
{
    Task<Movie> GetMovieById(int id);
    Task<MessageResponseDTO> CreateMovie(MovieRequestDTO movie);
    Task<IEnumerable<Movie>> GetAllMoviesList();
    Task<Movie?> UpdateMovie(int id, MovieRequestDTO requestDto);
    Task<MessageResponseDTO> DeleteMovie(int id);
}