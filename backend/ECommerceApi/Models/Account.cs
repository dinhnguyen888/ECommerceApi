
using Microsoft.AspNetCore.Identity;

namespace ECommerceApi.Models
{
    public class Account 
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string? PictureUrl { get; set; }
        public Role Role { get; set; }
        public int RoleId { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
        public Payment? Payment { get; set; }
    }
}
