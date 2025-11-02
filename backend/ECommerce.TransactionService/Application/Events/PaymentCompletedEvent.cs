namespace ECommerce.TransactionService.Application.Events
{
    public class PaymentCompletedEvent
    {
        public string PaymentId { get; set; }
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidAt { get; set; }
    }
}
