using System;
using System.Collections.Generic;

namespace ECommerce.TransactionService.Application.Entities
{
    // Entity cho Order - don hang
    public class Order
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ExpiresAt { get; set; } // Thoi gian het han thanh toan (30 phut)

        // Navigation properties
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }

    // Enum trang thai don hang
    public enum OrderStatus
    {
        Pending,  // Cho thanh toan
        Paid,     // Da thanh toan
        Failed    // Thanh toan that bai
    }
}
