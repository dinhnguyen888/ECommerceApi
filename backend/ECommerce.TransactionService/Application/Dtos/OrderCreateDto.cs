using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.TransactionService.Application.Dtos
{
    // DTO de tao don hang moi
    public class OrderCreateDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Don hang phai co it nhat 1 san pham")]
        public List<OrderItemCreateDto> OrderItems { get; set; }
    }

    // DTO de tao chi tiet don hang
    public class OrderItemCreateDto
    {
        [Required]
        public string ProductId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Gia phai lon hon 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "So luong phai lon hon 0")]
        public int Quantity { get; set; } = 1;
    }
}
