using System.ComponentModel.DataAnnotations;

namespace Movies.dto;

public class TheatreRequestDTO
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Location { get; set; }
    [Required]
    public long Capacity { get; set; }
}
