using System;

namespace ECommerce.TransactionService.Application.Events
{
    // Event de gui thong bao thanh toan thanh cong den NotificationService
    public class PaymentSuccessNotificationEvent
    {
        public string PaymentId { get; set; }
        public string OrderId { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidAt { get; set; }
    }
}
