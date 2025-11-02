using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.TransactionService.Application.Dtos;

namespace ECommerce.TransactionService.Application.Interfaces
{
    // Interface cho Order Service - business logic cua Order
    public interface IOrderService
    {
        // Tao don hang moi
        Task<OrderGetDto> CreateOrderAsync(OrderCreateDto dto);

        // Lay don hang theo ID
        Task<OrderGetDto?> GetOrderByIdAsync(string id);

        // Lay tat ca don hang cua user
        Task<List<OrderGetDto>> GetOrdersByUserIdAsync(string userId);

        // Lay tat ca don hang (admin)
        Task<List<OrderGetDto>> GetAllOrdersAsync();

        // Cap nhat trang thai don hang
        Task UpdateOrderStatusAsync(string orderId, Entities.OrderStatus status);
    }
}
