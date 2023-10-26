using System.ComponentModel.DataAnnotations;

namespace Movies.dto;

public class GenreBulkRequest
{
    [Required]
    public List<string> Genres { get; set; } = new();
}