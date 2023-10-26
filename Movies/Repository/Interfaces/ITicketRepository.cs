using Movies.Model;

namespace Movies.Repository;

public interface ITicketRepository
{
    Task<Ticket> CreateTicket(Ticket ticket);
    Task saveChanges();
}