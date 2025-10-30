namespace ECommerce.AuthService.Application.Dtos
{
    public class UserCreateDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "Client";
        public bool IsActive { get; set; } = true;
    }
}
