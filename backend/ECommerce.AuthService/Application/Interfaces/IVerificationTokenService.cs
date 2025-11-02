namespace ECommerce.AuthService.Application.Interfaces
{
    public interface IVerificationTokenService
    {
        string GenerateVerificationToken(string userId, string email);
        (bool isValid, string? userId, string? email) ValidateVerificationToken(string token);
    }
}

