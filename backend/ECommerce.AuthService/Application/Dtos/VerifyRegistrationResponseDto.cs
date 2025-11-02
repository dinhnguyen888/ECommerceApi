namespace ECommerce.AuthService.Application.Dtos
{
    public class VerifyRegistrationResponseDto
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public UserDto? User { get; set; }
    }
}

