using Movies.dto;
using Movies.Model;

namespace Movies.Services.Interfaces;

public interface IMovieShowService
{
    Task<List<MovieShow>> GetAllMovieShows();
    Task<MovieShow> GetMovieShowById(int id);
    Task<MovieShow> UpdateMovieShow(int id, MovieShowRequestDto movieShowRequestDto);
    Task<MessageResponseDTO> CreateMovieShow(MovieShowRequestDto movieShowRequestDto);
    Task<MessageResponseDTO> DeleteMovieShow(int id);
}