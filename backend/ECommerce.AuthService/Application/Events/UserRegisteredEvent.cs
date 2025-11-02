namespace ECommerce.AuthService.Application.Events
{
    public class UserRegisteredEvent
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime RegisteredAt { get; set; }
    }
}
