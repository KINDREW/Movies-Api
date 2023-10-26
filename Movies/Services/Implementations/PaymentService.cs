using System.Globalization;
using System.Text;
using Movies.dto;
using Movies.Exceptions;
using Movies.Repository.Interfaces;
using Movies.Services.Interfaces;
using Newtonsoft.Json;

namespace Movies.Services.Implementations;

public class PaymentService: IPaymentService
{
    private readonly IMovieEventBookingRepository _movieEventBookingRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PaymentService> _logger;
    public PaymentService(IMovieEventBookingRepository movieEventBookingRepository, IConfiguration configuration, ILogger<PaymentService> logger)
    {
        _movieEventBookingRepository = movieEventBookingRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<PaymentMessageResponseDto> InitiatePayment(PaymentDto paymentDto)
    {
        var movieEvent = await _movieEventBookingRepository.GetBookingById(paymentDto.PaymentReference);
        if (movieEvent == null)
        {
            throw new NotFound404Exception("payment reference not valid");
        }
        if (movieEvent.IsPaid)
        {
            throw new BadRequest400Exception("payment already made for this booking");
        }
        var payStackApiKey = _configuration.GetSection("payStack:apikey").Value!;
        var paymentInitialize = new PaymentInitializeDto
        {
            email = movieEvent.EmailAddress,
            amount = (movieEvent.AmountPayable * 100).ToString(CultureInfo.InvariantCulture),
            reference = paymentDto.PaymentReference.ToString()
        };
        var requestDataJson = Newtonsoft.Json.JsonConvert.SerializeObject(paymentInitialize);
        using (var client = new HttpClient())
        {
            var payStackUrl = new Uri("https://api.paystack.co/transaction/initialize");
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {payStackApiKey}");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var response = await client.PostAsync(payStackUrl, new StringContent(requestDataJson, Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                throw new BadRequest400Exception("payment initialization failed: payment already initialized for this reference");
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            var paymentResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<PayStackPaymentInitializerResponseDto>(responseContent);
            return new PaymentMessageResponseDto
            {
                status = "success",
                message = "payment initiation success, please approve the payment using the authorization url provided",
                AccessCode = paymentResponse.data.access_code,
                AuthorizationUrl = paymentResponse.data.authorization_url,
                Reference = paymentResponse.data.reference
            };
        }
    }
    public async Task ProcessPaymentWebhookResponse(PaymentWebHookResponse response)
    {
        _logger.LogInformation("user payment with reference {}, completed successfully. Account credited",response.Data.Reference);
        if (response.Events == null)
        {
            _logger.LogInformation("webhook response not valid: {}",response );
            return;
        }
        if (response.Events == "charge.success")
        {
            var bookedEvent = await _movieEventBookingRepository.GetBookingById(Convert.ToInt32(response.Data.Reference));
            if (bookedEvent == null)
            {
                _logger.LogError("reference for payment not found. Reference is: {}",response.Data.Reference);
                return;
            }

            if (bookedEvent.IsPaid)
            {
                _logger.LogInformation("payment with reference {}, already made payment",response.Data.Reference);
                return;
            }
            bookedEvent.IsPaid = true;
            await _movieEventBookingRepository.SaveChanges();
            return;
        }
        _logger.LogInformation("user payment with reference {}, not completed. Payment not made",response.Data.Reference);
    }
}