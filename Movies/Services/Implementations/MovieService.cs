using Movies.dto;
using Movies.Exceptions;
using Movies.Model;
using Movies.Repository;
using Movies.Repository.Interfaces;

namespace Movies.Services.Implementations;

public class MovieService:IMovieService
{
    private readonly IMoviesRepository _moviesRepository;
    private readonly IGenreRepository _genreRepository;

    public MovieService(IMoviesRepository moviesRepository, IGenreRepository genreRepository)
    {
        _moviesRepository = moviesRepository;
        _genreRepository = genreRepository;
    }

    public async Task<Movie> GetMovieById(int id)
    {
        var movie= await _moviesRepository.GetMovieById(id);
        if (movie == null)
        {
            throw new NotFound404Exception("movie no found");
        }
        
        return movie;
    }

    public async Task<MessageResponseDTO> CreateMovie(MovieRequestDTO movie)
    {
        var savedMovie = await _moviesRepository.GetMovieByName(movie.Title.ToLower());
        if (savedMovie is not null)
        {
            throw new Duplicate409Exception("movie already created");
        }
        var newMovie = new Movie
        {
            Description = movie.Description,
            Title = movie.Title.ToLower(),
            ReleasedDate = movie.ReleasedDate
        };
        foreach (var genre in movie.GenreIdsList)
        {
            var savedGenre = await _genreRepository.GetGenreById(genre);
            if (savedGenre != null)
            {
                newMovie.Genres.Add(savedGenre);
            }
        }
        await _moviesRepository.AddMovie(newMovie);
        return new MessageResponseDTO()
        {
            message = "movie saved",
            status = "success"
        };
    }

    public async Task<IEnumerable<Movie>> GetAllMoviesList()
    {
        return await _moviesRepository.GetAllMovies();
    }

    public async Task<Movie?> UpdateMovie(int id, MovieRequestDTO requestDto)
    {
        var savedMovie = await _moviesRepository.GetMovieById(id);
        if (savedMovie == null)
        {
            throw new NotFound404Exception("movie not found");
        }
        foreach (var genre in requestDto.GenreIdsList)
        {
            var savedGenre = await _genreRepository.GetGenreById(genre);
            if (savedGenre != null && !savedMovie.Genres.Contains(savedGenre))
            {
                savedMovie.Genres.Add(savedGenre);
            }
        }
        savedMovie.Description = requestDto.Description;
        savedMovie.Title = requestDto.Title;
        savedMovie.ReleasedDate = requestDto.ReleasedDate;
        savedMovie.UpdatedAt = DateTime.UtcNow;
        _moviesRepository.SaveChanges();
        return savedMovie;
    }

    public async Task<MessageResponseDTO> DeleteMovie(int id)
    {
        var movie = await _moviesRepository.GetMovieById(id);
        if (movie == null)
        {
            throw new NotFound404Exception("movie not found");
        }
        _moviesRepository.DeleteMovie(movie);
        return new MessageResponseDTO()
        {
            message = "movie deleted successfully",
            status = "success"
        };
    }
}
