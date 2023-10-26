using Movies.dto;
using Movies.Exceptions;
using Movies.Model;
using Movies.Repository;
using Movies.Repository.Interfaces;
using Movies.Services.Interfaces;

namespace Movies.Services.Implementations;

public class MovieShowService:IMovieShowService
{
    private readonly IMovieShowRepository _movieShowRepository;
    private readonly IMoviesRepository _moviesRepository;
    private readonly ITheatreRepository _theatreRepository;
    private readonly ITicketRepository _ticketRepository;

    public MovieShowService(IMovieShowRepository movieShowRepository, IMoviesRepository moviesRepository, ITheatreRepository theatreRepository, ITicketRepository ticketRepository)
    {
        _movieShowRepository = movieShowRepository;
        _moviesRepository = moviesRepository;
        _theatreRepository = theatreRepository;
        _ticketRepository = ticketRepository;
    }

    public async Task<List<MovieShow>> GetAllMovieShows()
    {
        return await _movieShowRepository.GetAllMovieShows();
    }

    public async Task<MovieShow> GetMovieShowById(int id)
    {
        var movie =  await _movieShowRepository.GetMovieShowById(id);
        if (movie == null)
        {
            throw new NotFound404Exception("movie show not found");
        }

        return movie;
    }

    public async Task<MovieShow> UpdateMovieShow(int id, MovieShowRequestDto movieShowRequestDto)
    {
        var movieShow = await _movieShowRepository.GetMovieShowById(id);
        if (movieShow == null)
        {
            throw new NotFound404Exception("movie show not found");
        }

        if (movieShow.Movie.Id != movieShowRequestDto.Movie)
        {
            var movie =  await _moviesRepository.GetMovieById(id);
            if (movie == null)
            {
                throw new NotFound404Exception("movie not found");
            }
            movieShow.Movie = movie;
        }

        if (movieShow.Theatre.Id != movieShowRequestDto.Theatre)
        {
            var theatre = await _theatreRepository.GetTheatreById(id);
            if (theatre == null)
            {
                throw new NotFound404Exception("theatre not found");
            }

            movieShow.Theatre = theatre;
        }

        if (movieShow.StartTimeOfShow != movieShowRequestDto.StartTimeOfShow)
        {
            var movieShows = await _movieShowRepository.GetAllMovieShowsOnTheSameTheatreStartTimeAndEndTime(
                movieShowRequestDto.Theatre, movieShowRequestDto.StartTimeOfShow);
            if (movieShows.Any())
            {
                throw new Duplicate409Exception("movie show already created for the time");
            }

            movieShow.StartTimeOfShow = movieShowRequestDto.StartTimeOfShow;
            movieShow.EndTimeOfShow = movieShowRequestDto.EndTimeOfShow;
        }
        movieShow.ShowDescription = movieShowRequestDto.ShowDescription;
        movieShow.ShowTile = movieShowRequestDto.ShowTile;
        movieShow.Tickets.TicketPrice = movieShowRequestDto.TicketPrice;
        _movieShowRepository.SaveChanges();
        _ticketRepository.saveChanges();
        return movieShow;
    }

    public async Task<MessageResponseDTO> CreateMovieShow(MovieShowRequestDto movieShowRequestDto)
    {
        var movie = await _moviesRepository.GetMovieById(movieShowRequestDto.Movie);
        if (movie == null)
        {
            throw new NotFound404Exception("movie does not exist");
        }

        var theatre = await _theatreRepository.GetTheatreById(movieShowRequestDto.Theatre);
        if (theatre == null)
        {
            throw new NotFound404Exception("theatre not found");
        }

        var movieShowsThatCrashWithTimeInTheSameTheatre = await _movieShowRepository
            .GetAllMovieShowsOnTheSameTheatreStartTimeAndEndTime
                (theatre.Id,movieShowRequestDto.StartTimeOfShow);
        if (movieShowsThatCrashWithTimeInTheSameTheatre.Any())
        {
            throw new Duplicate409Exception("movie event already crated for that time, please choose another time");
        }
        var movieShow = new MovieShow
        {
            ShowTile = movieShowRequestDto.ShowTile,
            ShowDescription = movieShowRequestDto.ShowDescription,
            StartTimeOfShow = movieShowRequestDto.StartTimeOfShow,
            EndTimeOfShow = movieShowRequestDto.EndTimeOfShow,
            Movie = movie,
            Theatre = theatre
        };
        var savedMovieShow =  await _movieShowRepository.CreateMovieShow(movieShow);
        var ticket = new Ticket()
        {
            
            TicketPrice = movieShowRequestDto.TicketPrice,
            MovieShow = savedMovieShow,
            TotalTicketsAvailable = (int) theatre.Capacity,
            NumberOfTicketsSold = 0
        };
        await _ticketRepository.CreateTicket(ticket);
        return new MessageResponseDTO
        {
            message = "movie show create success",
            status = "success"
        };
    }

    public async Task<MessageResponseDTO> DeleteMovieShow(int id)
    {
        var movieShow = await _movieShowRepository.GetMovieShowById(id);
        if (movieShow == null)
        {
            throw new NotFound404Exception("movie show not found");
        }
        _movieShowRepository.DeleteMovieShow(movieShow);
        return new MessageResponseDTO
        {
            message = "movie show deleted",
            status = "success"
        };
    }
}