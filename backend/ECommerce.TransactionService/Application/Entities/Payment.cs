using System;

namespace ECommerce.TransactionService.Application.Entities
{
    // Entity cho Payment - thanh toan
    public class Payment
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; } // Trang thai thanh toan
        public string TransactionId { get; set; } // ID giao dich tu VNPay hoac cac cong thanh toan khac
        public DateTime? PaidAt { get; set; } // Thoi gian thanh toan thanh cong
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal Amount { get; set; }
        public string? FailureReason { get; set; } // Ly do that bai neu co

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

    // Enum trang thai thanh toan
    public enum PaymentStatus
    {
        Pending,    // Dang cho thanh toan
        Processing, // Dang xu ly
        Completed,  // Thanh toan thanh cong
        Failed      // Thanh toan that bai
    }
}
