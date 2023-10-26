using Movies.dto;
using Movies.Exceptions;
using Movies.Model;
using Movies.Repository;
using Movies.Repository.Interfaces;

namespace Movies.Services.Implementations;

public class GenreService: IGenreService
{
    private readonly IGenreRepository _repository;
    private readonly ILogger<GenreService> _logger;

    public GenreService(IGenreRepository repository, ILogger<GenreService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<MessageResponseDTO> AddGenre(GenreRequestDTO requestDto)
    {
        _logger.LogInformation("creating genre with name {}",requestDto.Name);
        if (await _repository.GetGenreByName(requestDto.Name.ToLower()) != null)
        {
            _logger.LogInformation("Creating genre with name {} failed. Already created",requestDto.Name);
            throw new Duplicate409Exception("genre already added");
        }

        var genre = new Genre
        {
            Name = requestDto.Name.ToLower()
        };
        await _repository.AddGenre(genre);
        _logger.LogInformation("creating genre with name {} success",requestDto.Name);
        return new MessageResponseDTO
        {
            message = "genre added successfully",
            status = "success"
        };
    }

    public async Task<MessageResponseDTO> UpdateGenre(int id, GenreRequestDTO requestDto)
    {
        _logger.LogInformation("updating genre with id {}", id);
        var genre = await _repository.GetGenreById(id);
        if (genre == null)
        {
            _logger.LogInformation("updating genre with id {} failure, not found", id);
            throw new NotFound404Exception("genre not found");
        }

        if (!genre.Name.Equals(requestDto.Name.ToLower()))
        {
            var alreadySavedGenre = await _repository.GetGenreByName(requestDto.Name.ToLower());
            if (alreadySavedGenre != null)
            {
                _logger.LogInformation("updating genre with id {} failure, duplicate found", id);
                throw new Duplicate409Exception("genre already saved with that name");
            }

            genre.Name = requestDto.Name;
            _repository.SaveChanges();
        }
        _logger.LogInformation("updating genre with id {} success", id);
        return new MessageResponseDTO
        {
            message = "genre update success",
            status = "success"
        };
    }

    public async Task<IEnumerable<Genre>> GetAllGenre()
    {
        _logger.LogInformation("fetching all genres");
        return await _repository.GetAllGenre();
    }

    public async Task<MessageResponseDTO> DeleteGenre(int id)
    {
        _logger.LogInformation("updating genre with id {}", id);
        var genre = await _repository.GetGenreById(id);
        if (genre == null)
        {
            _logger.LogInformation("updating genre with id {} failure, not found", id);
            throw new NotFound404Exception("genre not found");
        }
        _repository.DeleteGenre(genre);
        _logger.LogInformation("deleting genre with id {} success", id);
        return new MessageResponseDTO
        {
            message = "genre delete success",
            status = "success"
        };
    }

    public async Task<MessageResponseDTO> AddBulkGenre(GenreBulkRequest genreBulkRequest)
    {
        _logger.LogInformation("adding bulk genres with ids {}", genreBulkRequest.Genres.ToString());
        foreach (var genre in genreBulkRequest.Genres)
        {
            var savedGenre = await _repository.GetGenreByName(genre.ToLower());
            if (savedGenre != null) continue;
            var newGenre = new Genre
            {
                Name = genre.ToLower()
            };
            await _repository.AddGenre(newGenre);
        }
        _logger.LogInformation("adding bulk genres with ids {} success", genreBulkRequest.Genres.ToString());
        return new MessageResponseDTO
        {
            message = "bulk genre add success",
            status = "success"
        };
    }
}