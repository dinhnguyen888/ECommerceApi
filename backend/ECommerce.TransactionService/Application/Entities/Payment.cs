using System;

namespace ECommerce.TransactionService.Application.Entities
{
    // Entity cho Payment - thanh toan
    public class Payment
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string TransactionId { get; set; } // ID giao dich tu VNPay hoac cac cong thanh toan khac
        public DateTime? PaidAt { get; set; } // Thoi gian thanh toan thanh cong
        public decimal Amount { get; set; }

        // Navigation properties
        public Order Order { get; set; }
    }

    // Enum phuong thuc thanh toan
    public enum PaymentMethod
    {
        Card,    // The tin dung
        PayPal,  // PayPal
        Momo,    // Vi dien tu MoMo
        Vnpay    // VNPay
    }
}
