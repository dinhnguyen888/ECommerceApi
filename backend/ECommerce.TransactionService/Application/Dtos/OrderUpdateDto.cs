using System.ComponentModel.DataAnnotations;
using ECommerce.TransactionService.Application.Entities;

namespace ECommerce.TransactionService.Application.Dtos
{
    public class OrderUpdateDto
    {
        [Required]
        public string Id { get; set; }

        public OrderStatus? Status { get; set; }

        public decimal? TotalAmount { get; set; }
    }
}
