using Movies.dto;
using Movies.Exceptions;
using Movies.Model;
using Movies.Repository;
using Movies.Repository.Interfaces;
using Movies.Services.Interfaces;

namespace Movies.Services.Implementations;

public class TheatreService: ITheatreService
{
    private readonly ITheatreRepository _theatreRepository;
    private readonly IMoviesRepository _moviesRepository;

    public TheatreService(ITheatreRepository theatreRepository, IMoviesRepository moviesRepository)
    {
        _theatreRepository = theatreRepository;
        _moviesRepository = moviesRepository;
    }

    public async Task<MessageResponseDTO> CreateTheatre(TheatreRequestDTO theatre)
    {
        var savedTheatre = await _theatreRepository
            .GetTheatreByNameAndLocation(theatre.Name.ToLower(), theatre.Location.ToLower());
        if (savedTheatre != null)
        {
            throw new Duplicate409Exception("theatre already created");
        }

        var newTheatre = new Theatre()
        {
            Name = theatre.Name.ToLower(),
            Capacity = theatre.Capacity,
            Location = theatre.Location.ToLower()
        };
        await _theatreRepository.AddTheatre(newTheatre);
        return new MessageResponseDTO()
        {
            message = "theatre save success",
            status = "success"
        };
    }

    public async Task<Theatre> GetTheatreById(int id)
    {
        var theatre = await _theatreRepository.GetTheatreById(id);
        if (theatre == null)
        {
            throw new NotFound404Exception("theatre not found");
        }

        return theatre;
    }

    public async Task<Theatre> UpdateTheatre(int id, TheatreRequestDTO theatreRequestDto)
    {
        var savedTheatre = await _theatreRepository.GetTheatreById(id);
        if (savedTheatre == null)
        {
            throw new NotFound404Exception("theatre not found");
        }

        savedTheatre.Capacity = theatreRequestDto.Capacity;
        savedTheatre.Location = theatreRequestDto.Location;
        savedTheatre.Name = theatreRequestDto.Name;
        _theatreRepository.SaveChanges();
        return savedTheatre;
    }

    public async Task<MessageResponseDTO> DeletedTheatreById(int id)
    {
        var theatre = await _theatreRepository.GetTheatreById(id);
        if (theatre == null)
        {
            throw new NotFound404Exception("theatre not found");
        }
        _theatreRepository.DeleteTheatre(theatre);
        return new MessageResponseDTO()
        {
            message = "theatre delete success",
            status = "success"
        };
    }

    public async Task<IEnumerable<Theatre>> GetAllTheatres()
    {
        return await _theatreRepository.GetAllTheatres();
    }

    public async Task<MessageResponseDTO> AddMovieToTheatre(int theatreId, AddMovieToTheatreRequestDto movieId)
    {
        var theatre = await _theatreRepository.GetTheatreById(theatreId);
        if (theatre == null)
        {
            throw new NotFound404Exception("theatre not found");
        }

        var movie = await _moviesRepository.GetMovieById(movieId.MovieId);
        if (movie == null)
        {
            throw new NotFound404Exception("movie not found");
        }

        if (theatre.Movies.Contains(movie))
        {
            throw new Duplicate409Exception("movie already added");
        }
        theatre.Movies.Add(movie);
        movie.Theatres.Add(theatre);
        _theatreRepository.SaveChanges();
        _moviesRepository.SaveChanges();
        return new MessageResponseDTO
        {
            message = "movie add to theatre success",
            status = "success"
        };
    }

    public async Task<MessageResponseDTO> AddBulkMovieToTheatre(int theatreId, List<int> movieIds)
    {
        var theatre = await _theatreRepository.GetTheatreById(theatreId);
        if (theatre == null)
        {
            throw new NotFound404Exception("theatre not found");
        }
        foreach (var movieId in movieIds)
        {
            var movie =await _moviesRepository.GetMovieById(movieId);
            if (movie != null && !theatre.Movies.Contains(movie))
            {
                theatre.Movies.Add(movie);
                movie.Theatres.Add(theatre);
                _moviesRepository.SaveChanges();
                _theatreRepository.SaveChanges();
            }
        }
        return new MessageResponseDTO()
        {
            message = "bulk movies added",
            status = "success"
        };
    }

    public async Task<IEnumerable<Movie>> GetAllMoviesInATheatre(int id)
    {
        var theatre = await _theatreRepository.GetTheatreById(id);
        if (theatre == null)
        {
            throw new NotFound404Exception("theatre not found");
        }

        return theatre.Movies;
    }

    public async Task<MessageResponseDTO> DeleteMovieFromTheatre(int theatreId, int movieId)
    {
        var theatre = await _theatreRepository.GetTheatreById(theatreId);
        if (theatre == null)
        {
            throw new NotFound404Exception("theatre not found");
        }

        var movie = await _moviesRepository.GetMovieById(movieId);
        if (movie == null)
        {
            throw new NotFound404Exception("movie not found");
        }

        if (!theatre.Movies.Contains(movie))
        {
            throw new NotFound404Exception("movie can not be found in theatre");
        }

        theatre.Movies.Remove(movie);
        movie.Theatres.Remove(theatre);
        _moviesRepository.SaveChanges();
        _theatreRepository.SaveChanges();
        return new MessageResponseDTO()
        {
            message = "movie remove success",
            status = "success"
        };
    }
}