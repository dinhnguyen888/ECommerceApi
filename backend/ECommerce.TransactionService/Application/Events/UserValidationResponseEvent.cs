namespace ECommerce.TransactionService.Application.Events
{
    // Event tra ve ket qua check user tu AuthService
    public class UserValidationResponseEvent
    {
        public string RequestId { get; set; } // ID de match request
        public string UserId { get; set; }
        public bool IsValid { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
