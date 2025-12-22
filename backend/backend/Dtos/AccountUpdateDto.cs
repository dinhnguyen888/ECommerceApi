using backend.Models;

namespace backend.Dtos
{
    public class AccountUpdateDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? RoleId { get; set; }
    }
}
