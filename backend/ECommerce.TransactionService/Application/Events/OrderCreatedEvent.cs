namespace ECommerce.TransactionService.Application.Events
{
    public class OrderCreatedEvent
    {
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
