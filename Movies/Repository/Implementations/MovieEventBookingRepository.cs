using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.dto;
using Movies.Model;
using Movies.Repository.Interfaces;
using Movies.Services.Interfaces;

namespace Movies.Repository.Implementations;

public class MovieEventBookingRepository: IMovieEventBookingRepository
{
    private readonly DataContext _dataContext;

    public MovieEventBookingRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<MovieEventBooking> CreateAMovieEventBooking(MovieEventBooking movieEventBooking)
    {
        var savedMovieEventBooking = await _dataContext.MovieEventBookings.AddAsync(movieEventBooking);
        await _dataContext.SaveChangesAsync();
        return savedMovieEventBooking.Entity;
    }
    

    public async Task<IEnumerable<MovieEventBooking>> GetAllBookings()
    {
        return await _dataContext.MovieEventBookings
            .Include(a=>a.MovieShow)
            .ToListAsync();
    }

    public async Task<MovieEventBooking?> GetBookingById(int id)
    {
        return await _dataContext.MovieEventBookings
            .Include(a => a.MovieShow)
            .FirstOrDefaultAsync(a => a.Id== id);
    }

    public async Task SaveChanges()
    {
        await _dataContext.SaveChangesAsync();
    }

    public async Task<List<MovieEventBooking>> GetBookingsByEmail(string emailAddress)
    {
        return await _dataContext.MovieEventBookings
            .Include(a=>a.MovieShow.Movie)
            .Where(a => a.EmailAddress == emailAddress)
            .ToListAsync();
    }
}
