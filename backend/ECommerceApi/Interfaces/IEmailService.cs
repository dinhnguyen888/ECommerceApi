
public interface IEmailService
{
    Task<bool> SendEmail(string toEmail, string subject, string body);
}