namespace ECommerce.AuthService.Application.Dtos
{
    public class AdminChangePasswordDto
    {
        public string UserId { get; set; }
        public string NewPassword { get; set; }
    }
}
