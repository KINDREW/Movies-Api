using Movies.Model;

namespace Movies.Repository.Interfaces;

public interface IMovieShowRepository
{
    Task<List<MovieShow>> GetAllMovieShows();
    Task<MovieShow?> GetMovieShowById(int id);
    Task<MovieShow?> CreateMovieShow(MovieShow? movieShow);
    Task<List<MovieShow>> GetAllMovieShowsOnTheSameTheatreStartTimeAndEndTime(int theatreId, DateTime startDateTime);
    Task DeleteMovieShow(MovieShow movieShow);
    Task SaveChanges();
}