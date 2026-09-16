namespace ECommerceApi.Dtos
{
    public class TokenGenerateDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } 
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}
