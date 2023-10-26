using Movies.Data;
using Movies.Model;

namespace Movies.Repository.Implementations;

public class TicketRepository:ITicketRepository
{
    private readonly DataContext _dataContext;

    public TicketRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<Ticket> CreateTicket(Ticket ticket)
    {
        var savedTicket = _dataContext.Tickets.Add(ticket);
        await _dataContext.SaveChangesAsync();
        return savedTicket.Entity;
    }

    public async Task saveChanges()
    {
        await _dataContext.SaveChangesAsync();
    }
}