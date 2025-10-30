namespace ECommerce.AuthService.Application.Dtos
{
    public class LogoutRequest
    {
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
