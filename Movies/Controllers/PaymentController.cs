using Microsoft.AspNetCore.Mvc;
using Movies.dto;
using Movies.Services.Interfaces;

namespace Movies.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class PaymentController:ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }
    [HttpPost("initiate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> InitiatePayment([FromBody] PaymentDto paymentDto)
    {
        return Ok(await _paymentService.InitiatePayment(paymentDto));
    }
    [HttpPost("webhook")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> PaymentWebHook([FromBody] PaymentWebHookResponse request)
    {
        await _paymentService.ProcessPaymentWebhookResponse(request);
        return Ok();
    }
}