using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Movies.Model;
[Table(name:"movieshow")]
public class MovieShow
{
    [Required]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string? ShowTile{ get; set; }
    [Required]
    public Theatre Theatre { get; set; } = new();
    [Required]
    public Movie Movie { get; set; } = new();
    [Required]
    public DateTime StartTimeOfShow { get; set; }
    [Required]
    public DateTime EndTimeOfShow { get; set; }
    [Required]
    public string? ShowDescription { get; set; }
    [Required]
    public Ticket? Tickets { get; set; }
    [JsonIgnore]
    public List<MovieEventBooking?> MovieEventBookings { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
}