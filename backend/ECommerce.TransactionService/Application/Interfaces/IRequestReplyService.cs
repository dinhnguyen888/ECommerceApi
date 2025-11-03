using System;
using System.Threading.Tasks;

namespace ECommerce.TransactionService.Application.Interfaces
{
    // Interface cho Request-Reply pattern
    public interface IRequestReplyService
    {
        // Gui request va cho response (Request-Reply pattern)
        Task<TResponse> RequestAsync<TRequest, TResponse>(
            string queueName,
            TRequest request,
            TimeSpan? timeout = null) where TRequest : class where TResponse : class;
    }
}

