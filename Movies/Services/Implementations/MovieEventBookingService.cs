using Movies.dto;
using Movies.Exceptions;
using Movies.Model;
using Movies.Repository;
using Movies.Repository.Interfaces;
using Movies.Services.Interfaces;

namespace Movies.Services.Implementations;

public class MovieEventBookingService: IMovieEventBookingService
{
    private readonly IMovieEventBookingRepository _movieEventBookingRepository;
    private readonly IMovieShowRepository _movieShowRepository;
    private readonly ILogger<MovieEventBookingService> _logger;
    private readonly IEmailService _emailService;
    public MovieEventBookingService(IMovieEventBookingRepository movieEventBookingRepository, IMovieShowRepository movieShowRepository, ITicketRepository ticketRepository, ILogger<MovieEventBookingService> logger, IEmailService emailService)
    {
        _movieEventBookingRepository = movieEventBookingRepository;
        _movieShowRepository = movieShowRepository;
        _logger = logger;
        _emailService = emailService;
    }

    public async Task<MessageResponseDTO> CreateAMovieEventBooking(MovieEventBookingDto movieEventBookingDto)
    {
        _logger.LogInformation("creating movie show booking for {} number of person(s) with email {}", 
            movieEventBookingDto.NumberOfPersons,movieEventBookingDto.EmailAddress);
        var movieShow = await _movieShowRepository.GetMovieShowById(movieEventBookingDto.MovieEventId);
        if (movieShow == null)
        {
            _logger.LogInformation("movie event booking failure, movie not found");
            throw new NotFound404Exception("movie event not found");
        }

        if (movieEventBookingDto.NumberOfPersons > movieShow.Tickets.NumberOfTicketsLeft)
        {
            _logger.LogInformation("movie event booking failure, can't satisfy request, no more tickets");
            throw new Duplicate409Exception("ticket can't satisfy you ticket left: "+movieShow.Tickets.NumberOfTicketsLeft);
        }

        var movieBookingEvent = new MovieEventBooking
        {
            AmountPayable = movieShow.Tickets.TicketPrice * movieEventBookingDto.NumberOfPersons,
            EmailAddress = movieEventBookingDto.EmailAddress,
            IsPaid = false,
            NumberOfPersons = movieEventBookingDto.NumberOfPersons,
            MovieShow = movieShow,
            TicketNumber = movieShow.Tickets.NumberOfTicketsSold+1
        };
        var savedMovieEventBooking = await _movieEventBookingRepository.CreateAMovieEventBooking(movieBookingEvent);
        movieShow.MovieEventBookings.Add(savedMovieEventBooking);
        movieShow.Tickets.NumberOfTicketsSold += movieEventBookingDto.NumberOfPersons;
        await _movieEventBookingRepository.SaveChanges();
        await _movieShowRepository.SaveChanges();
        _logger.LogInformation("movie event booking success");
        var message = $"Dear {movieEventBookingDto.EmailAddress!.Split("@")[0]},\n" +
                      $" Movie Event Booked for {movieEventBookingDto.NumberOfPersons} person(s). \n" +
                      $"Please do well to make payment to secure tickets. Payment Reference Is: {savedMovieEventBooking.Id}. " +
                      $"Please use this reference when making the payment.";
        _emailService.SendEmail("Movie Event Booking", movieEventBookingDto.EmailAddress,message);
        return new MessageResponseDTO
        {
            message = "movie event book success",
            status = "success"
        };
    }

    public async Task<IEnumerable<MovieEventBooking>> GetAllBookings()
    {
        _logger.LogInformation("fetching all bookings success");
        return await _movieEventBookingRepository.GetAllBookings();
    }

    public async Task<MovieEventBooking> GetBookingById(int id)
    {
        _logger.LogInformation("getting booking with Id {}",id);
        var movieEventBooking = await _movieEventBookingRepository.GetBookingById(id);
        if (movieEventBooking == null)
        {
            _logger.LogInformation("getting booking with Id {} failure, booking not found",id);
            throw new NotFound404Exception("movie event booking not found");
        }
        _logger.LogInformation("getting booking with Id {} success",id);
        return movieEventBooking;
    }

    public async Task<List<MovieEventBooking>> GetBookingsByEmail(string emailAddress)
    {
        return await _movieEventBookingRepository.GetBookingsByEmail(emailAddress);
    }
}
