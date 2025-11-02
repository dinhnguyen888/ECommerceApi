using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.TransactionService.Infrastructure.Configurations
{
    // Extension methods cho Infrastructure configuration
    // File nay co the mo rong de them cache, message queue, etc.
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register infrastructure services here (DB, MQ, Cache, etc.)
            // Hien tai DB duoc config trong Program.cs
            return services;
        }
    }
}


