using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Movies.Data;
using Movies.dto;
using Movies.Model;
using Movies.Repository.Interfaces;

namespace Movies.Repository.Implementations;

public class MovieShowRepository: IMovieShowRepository
{
    private readonly DataContext _dataContext;

    public MovieShowRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<List<MovieShow>> GetAllMovieShows()
    {
        return await _dataContext.MovieShows
            .Include(a => a.Movie)
            .Include(a => a.Theatre)
            .Include(a=>a.Tickets)
            .ToListAsync();
    }

    public async Task<MovieShow?> GetMovieShowById(int id)
    {
        return await _dataContext.MovieShows
            .Include(a => a.Tickets)
            .Include(a=>a.Theatre)
            .Include(a=>a.Movie)
            .FirstOrDefaultAsync(a => a.Id == id);
        
    }
    public async Task<MovieShow?> CreateMovieShow(MovieShow? movieShow)
    {
        var savedMovieShow = await _dataContext.MovieShows.AddAsync(movieShow);
        await _dataContext.SaveChangesAsync();
        return savedMovieShow.Entity;
    }

    public async Task DeleteMovieShow(MovieShow movieShow)
    {
        _dataContext.MovieShows.Remove(movieShow);
        await _dataContext.SaveChangesAsync();
    }

    public async Task SaveChanges()
    { 
        await _dataContext.SaveChangesAsync();
    }

    public async Task<List<MovieShow>> GetAllMovieShowsOnTheSameTheatreStartTimeAndEndTime(int theatreId, DateTime startDateTime)
    {
        return await _dataContext.MovieShows
            .Include(a => a.Theatre)
            .Where(a => a.Theatre.Id == theatreId && startDateTime >= a.StartTimeOfShow &&
                        startDateTime <= a.EndTimeOfShow)
            .ToListAsync();
    }
}