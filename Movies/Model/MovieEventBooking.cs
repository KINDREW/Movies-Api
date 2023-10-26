using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Model;
[Table(name:"movieeventbooking")]
public class MovieEventBooking
{
    [Required]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string? EmailAddress { get; set; }
    [Required]
    public int TicketNumber { get; set; }
    [Required]
    public int NumberOfPersons { get; set; }
    [Required]
    public double AmountPayable { get; set; }
    public bool IsPaid { get; set; }
    public MovieShow MovieShow { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}