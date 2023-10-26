using System.ComponentModel.DataAnnotations;

namespace Movies.dto;

public class PaymentDto
{
    [Required]
    public int PaymentReference { get; set; }
}