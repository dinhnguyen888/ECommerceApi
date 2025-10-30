namespace ECommerce.AuthService.Application.Dtos
{
    public class DeactivateAccountDto
    {
        public string AccessToken { get; set; }
        public string UserId { get; set; }
        public bool Deactivate { get; set; } = true;
    }
}
