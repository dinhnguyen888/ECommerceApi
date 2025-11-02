using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.TransactionService.Application.Dtos;
using ECommerce.TransactionService.Application.Entities;
using ECommerce.TransactionService.Application.Interfaces;

namespace ECommerce.TransactionService.Application.Services
{
    // Service xu ly business logic cua Order
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepo, IMapper mapper)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
        }

        // Tao don hang moi
        public async Task<OrderGetDto> CreateOrderAsync(OrderCreateDto dto)
        {
            // Tinh tong tien
            var totalAmount = dto.OrderItems.Sum(item => item.Price * item.Quantity);

            // Tao Order entity
            var order = new Order
            {
                Id = Guid.NewGuid().ToString(),
                UserId = dto.UserId,
                TotalAmount = totalAmount,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            // Tao OrderItems
            foreach (var itemDto in dto.OrderItems)
            {
                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid().ToString(),
                    OrderId = order.Id,
                    ProductId = itemDto.ProductId,
                    Price = itemDto.Price,
                    Quantity = itemDto.Quantity
                };
                order.OrderItems.Add(orderItem);
            }

            // Luu vao database
            var createdOrder = await _orderRepo.CreateAsync(order);

            // Tra ve DTO
            return _mapper.Map<OrderGetDto>(createdOrder);
        }

        // Lay don hang theo ID
        public async Task<OrderGetDto?> GetOrderByIdAsync(string id)
        {
            var order = await _orderRepo.GetByIdWithDetailsAsync(id);
            if (order == null)
                return null;

            return _mapper.Map<OrderGetDto>(order);
        }

        // Lay tat ca don hang cua user
        public async Task<List<OrderGetDto>> GetOrdersByUserIdAsync(string userId)
        {
            var orders = await _orderRepo.GetByUserIdAsync(userId);
            return _mapper.Map<List<OrderGetDto>>(orders);
        }

        // Lay tat ca don hang (admin)
        public async Task<List<OrderGetDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepo.GetAllAsync();
            return _mapper.Map<List<OrderGetDto>>(orders);
        }

        // Cap nhat trang thai don hang
        public async Task UpdateOrderStatusAsync(string orderId, OrderStatus status)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null)
                throw new ArgumentException($"Don hang khong ton tai: {orderId}");

            order.Status = status;
            await _orderRepo.UpdateAsync(order);
        }
    }
}
