namespace ECommerce.AuthService.Application.Dtos
{
    public class ChangePasswordDto
    {
        public string AccessToken { get; set; }
        public string UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
