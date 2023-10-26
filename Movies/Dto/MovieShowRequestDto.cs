using System.ComponentModel.DataAnnotations;
using Movies.Model;

namespace Movies.dto;

public class MovieShowRequestDto
{
    [Required]
    public string? ShowTile{ get; set; }
    [Required]
    public int Theatre { get; set; } = new();
    [Required]
    public int Movie { get; set; } = new();
    [Required]
    public DateTime StartTimeOfShow { get; set; }
    [Required]
    public DateTime EndTimeOfShow { get; set; }
    [Required]
    public string? ShowDescription { get; set; }
    public double TicketPrice { get; set; }
}