using System.ComponentModel.DataAnnotations;

namespace Movies.dto;

public class AddMovieToTheatreRequestDto
{
    [Required]
    public int MovieId { get; set; }
}