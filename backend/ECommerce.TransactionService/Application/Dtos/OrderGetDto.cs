using System;
using System.Collections.Generic;
using ECommerce.TransactionService.Application.Entities;

namespace ECommerce.TransactionService.Application.Dtos
{
    // DTO de tra ve thong tin don hang
    public class OrderGetDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemGetDto> OrderItems { get; set; }
        public List<PaymentGetDto> Payments { get; set; }
    }

    // DTO de tra ve chi tiet don hang
    public class OrderItemGetDto
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
