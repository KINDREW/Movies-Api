namespace Movies.dto;

public class PaymentMessageResponseDto: MessageResponseDTO
{
    public string AuthorizationUrl { get; set; }
    public string AccessCode { get; set; }
    public string Reference { get; set; }
}