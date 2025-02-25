using ECommerceApi.Models;

namespace ECommerceApi.Dtos
{
    public class AccountGetDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
        public string PictureUrl { get; set; }
    }
}
