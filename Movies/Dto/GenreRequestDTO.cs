using System.ComponentModel.DataAnnotations;

namespace Movies.dto;

public class GenreRequestDTO
{
    [Required]
    public string Name { get; set; }
}