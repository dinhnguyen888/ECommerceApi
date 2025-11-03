namespace ECommerce.TransactionService.Application.Events
{
    // Event de yeu cau AuthService check thong tin user
    public class UserValidationRequestEvent
    {
        public string RequestId { get; set; } // ID de match response
        public string UserId { get; set; }
    }
}
