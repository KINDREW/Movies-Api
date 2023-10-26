using Movies.dto;
using Movies.Model;

namespace Movies.Services;

public interface IGenreService
{
    Task<MessageResponseDTO> AddGenre(GenreRequestDTO requestDto);
    Task<MessageResponseDTO> UpdateGenre(int id, GenreRequestDTO requestDto);
    Task<IEnumerable<Genre>> GetAllGenre();
    Task<MessageResponseDTO> DeleteGenre(int id);
    Task<MessageResponseDTO> AddBulkGenre(GenreBulkRequest genreBulkRequest);
}