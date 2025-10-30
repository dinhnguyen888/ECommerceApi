namespace ECommerce.AuthService.Application.Dtos
{
    public class LoginDto
    {
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
