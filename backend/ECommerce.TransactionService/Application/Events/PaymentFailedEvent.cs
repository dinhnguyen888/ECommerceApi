using System;

namespace ECommerce.TransactionService.Application.Events
{
    // Event khi thanh toan that bai
    public class PaymentFailedEvent
    {
        public string PaymentId { get; set; }
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string FailureReason { get; set; }
        public DateTime FailedAt { get; set; }
    }
}
