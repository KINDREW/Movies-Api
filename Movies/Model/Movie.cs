using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Movies.Model;
[Table(name:"movie")]
public class Movie
{
    [Required]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public string? Title { get; set; }
    [Required]
    public List<Genre>? Genres { get; set; }= new();
    [Required]
    public string? Description { get; set; }
    [Required]
    public DateTime ReleasedDate { get; set; }
    [JsonIgnore]
    public List<Theatre> Theatres { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
}