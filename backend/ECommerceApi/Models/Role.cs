namespace ECommerceApi.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}
