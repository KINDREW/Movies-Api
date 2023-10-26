using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.Model;

namespace Movies.Repository;

public interface IMoviesRepository
{
    Task<Movie?> GetMovieById(int id);
    Task<IEnumerable<Movie>> GetAllMovies();
    Task<Movie> AddMovie(Movie movie);
    Task<Movie?> GetMovieByName(string name);
    Task DeleteMovie(Movie movie);
    Task SaveChanges();
}

