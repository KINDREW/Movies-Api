using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Model;
[Table(name:"ticket")]
public class Ticket
{
    [Required]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public double TicketPrice { get; set; }
    [Required]
    public int TotalTicketsAvailable { get; set; }
    [Required]
    public int NumberOfTicketsSold { get; set; }
    [NotMapped]
    public int NumberOfTicketsLeft => TotalTicketsAvailable - NumberOfTicketsSold;
    [Required]
    public MovieShow? MovieShow { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
}