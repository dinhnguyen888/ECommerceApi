using backend.Models;

namespace backend.Dtos
{
    public class AccountPostDto
    {
        public string Email { get; set; }
        public string? Password { get; set; }
        public string Name { get; set; }
        public int? RoleId { get; set; }

    }
}
