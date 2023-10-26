using Microsoft.EntityFrameworkCore;
using Movies.Data;
using Movies.Model;
using Movies.Repository.Interfaces;

namespace Movies.Repository.Implementations;

public class GenreRepository: IGenreRepository
{
    private readonly DataContext _context;

    public GenreRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Genre?> AddGenre(Genre genre)
    {
         var savedGenre =await _context.Genres.AddAsync(genre);
         await _context.SaveChangesAsync();
        return savedGenre.Entity;
    }
    
    public async Task<Genre?> GetGenreById(int id)
    {
        return await _context.Genres.FindAsync(id);
    }

    public async Task<Genre?> GetGenreByName(string name)
    {
        var savedGenre =await _context.Genres.FirstOrDefaultAsync(t => t.Name == name);
        return savedGenre;
    }

    public async Task<IEnumerable<Genre>> GetAllGenre()
    {
        return await _context.Genres.ToListAsync();
    }

    public async Task DeleteGenre(Genre genre)
    {
        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}