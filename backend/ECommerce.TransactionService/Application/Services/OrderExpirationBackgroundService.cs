using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.TransactionService.Application.Interfaces;
using ECommerce.TransactionService.Application.Entities;
using ECommerce.TransactionService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ECommerce.TransactionService.Application.Services
{
    // Background service de xu ly timeout orders (30 phut)
    public class OrderExpirationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OrderExpirationBackgroundService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5); // Kiem tra moi 5 phut

        public OrderExpirationBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<OrderExpirationBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OrderExpirationBackgroundService da khoi dong");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessExpiredOrdersAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Loi khi xu ly expired orders");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }

        private async Task ProcessExpiredOrdersAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TransactionDbContext>();
            var orderRepo = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

            var expiredOrders = await orderRepo.GetExpiredPendingOrdersAsync();

            if (expiredOrders.Count == 0)
            {
                _logger.LogInformation("Khong co order nao het han");
                return;
            }

            _logger.LogInformation($"Tim thay {expiredOrders.Count} order(s) da het han");

            using var transaction = await db.Database.BeginTransactionAsync();
            try
            {
                foreach (var order in expiredOrders)
                {
                    // Xoa order items
                    var orderItems = await db.OrderItems
                        .Where(oi => oi.OrderId == order.Id)
                        .ToListAsync();

                    db.OrderItems.RemoveRange(orderItems);

                    // Xoa payments
                    var payments = await db.Payments
                        .Where(p => p.OrderId == order.Id)
                        .ToListAsync();

                    db.Payments.RemoveRange(payments);

                    // Xoa order
                    db.Orders.Remove(order);

                    _logger.LogInformation($"Da xoa order {order.Id} va cac ban ghi lien quan");
                }

                await db.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation($"Da xoa thanh cong {expiredOrders.Count} order(s) het han");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Loi khi xoa expired orders");
                throw;
            }
        }
    }
}

