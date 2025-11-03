using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.TransactionService.Application.Dtos;
using ECommerce.TransactionService.Application.Entities;
using ECommerce.TransactionService.Application.Interfaces;
using ECommerce.TransactionService.Application.Events;

namespace ECommerce.TransactionService.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        private readonly IMessagePublisher _messagePublisher;

        public OrderService(IOrderRepository orderRepo, IMapper mapper, IMessagePublisher messagePublisher)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
        }

        public async Task<OrderGetDto> CreateOrderAsync(OrderCreateDto dto)
        {
            var totalAmount = dto.OrderItems.Sum(item => item.Price * item.Quantity);

            var order = new Order
            {
                Id = Guid.NewGuid().ToString(),
                UserId = dto.UserId,
                TotalAmount = totalAmount,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30)
            };

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

            var createdOrder = await _orderRepo.CreateAsync(order);

            var orderCreatedEvent = new OrderCreatedEvent
            {
                OrderId = order.Id,
                UserId = order.UserId,
                TotalAmount = order.TotalAmount,
                CreatedAt = order.CreatedAt
            };
            _messagePublisher.PublishToQueue("order.created", orderCreatedEvent);

            return _mapper.Map<OrderGetDto>(createdOrder);
        }

        public async Task<OrderGetDto?> GetOrderByIdAsync(string id)
        {
            var order = await _orderRepo.GetByIdWithDetailsAsync(id);
            if (order == null)
                return null;

            return _mapper.Map<OrderGetDto>(order);
        }

        public async Task<List<OrderGetDto>> GetOrdersByUserIdAsync(string userId)
        {
            var orders = await _orderRepo.GetByUserIdAsync(userId);
            return _mapper.Map<List<OrderGetDto>>(orders);
        }

        public async Task<List<OrderGetDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepo.GetAllAsync();
            return _mapper.Map<List<OrderGetDto>>(orders);
        }

        public async Task UpdateOrderStatusAsync(string orderId, OrderStatus status)
        {
            var order = await _orderRepo.GetByIdAsync(orderId);
            if (order == null)
                throw new ArgumentException($"Don hang khong ton tai: {orderId}");

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;
            await _orderRepo.UpdateAsync(order);
        }

        public async Task<OrderGetDto> UpdateOrderAsync(OrderUpdateDto dto)
        {
            var order = await _orderRepo.GetByIdAsync(dto.Id);
            if (order == null)
                throw new ArgumentException($"Don hang khong ton tai: {dto.Id}");

            if (dto.Status.HasValue)
                order.Status = dto.Status.Value;

            if (dto.TotalAmount.HasValue)
                order.TotalAmount = dto.TotalAmount.Value;

            order.UpdatedAt = DateTime.UtcNow;
            await _orderRepo.UpdateAsync(order);

            var updatedOrder = await _orderRepo.GetByIdWithDetailsAsync(dto.Id);
            return _mapper.Map<OrderGetDto>(updatedOrder!);
        }

        public async Task DeleteOrderAsync(string id)
        {
            var order = await _orderRepo.GetByIdAsync(id);
            if (order == null)
                throw new ArgumentException($"Don hang khong ton tai: {id}");

            await _orderRepo.DeleteAsync(id);
        }
    }
}
