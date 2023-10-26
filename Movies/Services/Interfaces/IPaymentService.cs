using Movies.dto;

namespace Movies.Services.Interfaces;

public interface IPaymentService
{
    Task<PaymentMessageResponseDto> InitiatePayment(PaymentDto paymentDto);
    Task ProcessPaymentWebhookResponse(PaymentWebHookResponse response);
}