using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.Model;

namespace Movies.Repository.Implementations;

public class MoviesRepository : IMoviesRepository
{
    private readonly DataContext _dataContext;

    public MoviesRepository(DataContext context)
    {
        _dataContext = context;
    }

    public async Task<Movie?> GetMovieById(int id)
    {
        return await _dataContext.Movies
            .Include(t=>t.Theatres)
            .Include(t=>t.Genres)
            .FirstOrDefaultAsync(a=>a.Id==id);
    }

    public async Task<IEnumerable<Movie>> GetAllMovies()
    {
        var movies = await _dataContext.Movies
            .Include(t=>t.Genres)
            .ToListAsync();
        return movies;
    }

    public async Task<Movie> AddMovie(Movie movie)
    {
        var savedMovie = await _dataContext.Movies.AddAsync(movie);
        await _dataContext.SaveChangesAsync();
        return savedMovie.Entity;
    }

    public async Task<Movie?> GetMovieByName(string name)
    {
        return await _dataContext.Movies
            .SingleOrDefaultAsync(a => a.Title == name);
    }

    public async Task DeleteMovie(Movie movie)
    {
        _dataContext.Movies.Remove(movie);
        await _dataContext.SaveChangesAsync();
    }

    public async Task SaveChanges()
    {
        await _dataContext.SaveChangesAsync();
    }
}