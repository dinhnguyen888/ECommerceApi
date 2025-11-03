namespace ECommerce.AuthService.Application.Events
{
    public class EmailSentEvent
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string NotificationId { get; set; }
        public bool Success { get; set; }
        public DateTime SentAt { get; set; }
    }
}

