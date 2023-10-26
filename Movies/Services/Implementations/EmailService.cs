using Movies.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Movies.Services.Implementations;

public class EmailService:IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;
    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async void SendEmail(string subject, string email, string message)
    {
        _logger.LogInformation("Email sending to {} initiated",email);
        var apiKey = _configuration.GetSection("Sendgrid:apikey").Value!;
        var client = new SendGridClient(apiKey);
        var fromEmail = new EmailAddress(_configuration.GetSection("Sendgrid:fromEmail").Value!,"Emmanuel Okyere Gyateng");
        var toEmail = new EmailAddress(email, "");
        var htmlContent = $"<strong>{message}</strong>";
        var msg = MailHelper.CreateSingleEmail(from:fromEmail, to:toEmail, subject:subject, message,htmlContent);
        await client.SendEmailAsync(msg).ConfigureAwait(false);
        _logger.LogInformation("Email sending to {} completed",email);
    }
}