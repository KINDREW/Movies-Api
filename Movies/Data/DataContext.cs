using Microsoft.EntityFrameworkCore;
using Movies.Model;

namespace Movies.Data;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }
    public DbSet<Theatre> Theatres { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<MovieShow?> MovieShows { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<MovieEventBooking> MovieEventBookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MovieShow>()
            .HasOne(m => m.Tickets)
            .WithOne(t => t.MovieShow)
            .HasForeignKey<Ticket>(t => t.Id)
            .IsRequired(false);

        modelBuilder.Entity<MovieShow>()
            .HasMany(m => m.MovieEventBookings)
            .WithOne(t => t.MovieShow)
            .IsRequired();
    }
    
}