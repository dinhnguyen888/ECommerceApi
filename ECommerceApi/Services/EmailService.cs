using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmail(string toEmail, string subject, string body)
    {
        try
        {
            var email = _configuration["Smtp:Email"];
            var password = _configuration["Smtp:Password"];
            var smtpHost = _configuration["Smtp:Host"];
            var smtpPort = _configuration["Smtp:Port"];
            var smtpClient = new SmtpClient(smtpHost, int.Parse(smtpPort)); 
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential(email, password);

            var message = new MailMessage(email!, toEmail, subject, body);

            await smtpClient.SendMailAsync(message);
        }
        catch (SmtpException smtpEx)
        {
            // Handle SMTP-specific exceptions
            throw new Exception($"SMTP Error: {smtpEx.Message}");
        }
        catch (Exception ex)
        {
            // Handle general exceptions
            throw new Exception($"Error sending email: {ex.Message}");
        }
    }
}
