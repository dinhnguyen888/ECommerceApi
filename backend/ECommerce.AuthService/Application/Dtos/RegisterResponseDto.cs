using System;

namespace ECommerce.AuthService.Application.Dtos
{
    public class RegisterResponseDto
    {
        public string VerifyUrl { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime RegisteredAt { get; set; }
    }
}

