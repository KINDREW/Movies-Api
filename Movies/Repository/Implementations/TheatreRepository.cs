using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.Model;
using Movies.Repository.Interfaces;

namespace Movies.Repository.Implementations;

public class TheatreRepository : ITheatreRepository
{
    private readonly DataContext _dataContext;

    public TheatreRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<Theatre?> GetTheatreById(int id)
    {
        return await _dataContext.Theatres
            .Include(t=>t.Movies)
            .FirstOrDefaultAsync(t=>t.Id==id);
    }

    public async Task<Theatre> AddTheatre(Theatre theatre)
    {
        var savedTheatre =await _dataContext.Theatres.AddAsync(theatre);
        await _dataContext.SaveChangesAsync();
        return savedTheatre.Entity;
    }

    public async Task<IEnumerable<Theatre>> GetAllTheatres()
    {
        return await _dataContext.Theatres.ToListAsync();
    }

    public async Task DeleteTheatre(Theatre theatre)
    {
        _dataContext.Theatres.Remove(theatre);
        await _dataContext.SaveChangesAsync();
    }
    
    public async Task<Theatre?> GetTheatreByNameAndLocation(string name, string location)
    {
        var theatre = await _dataContext.Theatres
            .SingleOrDefaultAsync(a => a.Name == name && a.Location == location);
        return theatre;
    }

    public async Task SaveChanges()
    {
        await _dataContext.SaveChangesAsync();
    }
    
}