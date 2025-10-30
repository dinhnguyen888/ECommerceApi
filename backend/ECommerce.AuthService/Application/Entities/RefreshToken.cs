using System;

namespace ECommerce.AuthService.Application.Entities
{
    public class RefreshToken
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiredAt { get; set; }

        public User User { get; set; }
    }
}
