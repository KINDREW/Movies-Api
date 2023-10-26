namespace Movies.dto;

public class TheatreResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public long Capacity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}