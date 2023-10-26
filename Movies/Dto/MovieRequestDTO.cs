using System.ComponentModel.DataAnnotations;

namespace Movies.dto;

public class MovieRequestDTO
{
    [Required]
    public string? Title { get; set; }

    [Required] public List<int>? GenreIdsList { get; set; } = new();
    [Required]
    public string? Description { get; set; }
    [Required]
    public DateTime ReleasedDate { get; set; }
}