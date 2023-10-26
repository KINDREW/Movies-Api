namespace Movies.Services.Interfaces;

public interface IEmailService
{
    void SendEmail(string subject, string toEmail, string plainTextContent);
}