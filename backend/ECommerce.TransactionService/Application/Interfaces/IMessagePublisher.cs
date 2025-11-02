namespace ECommerce.TransactionService.Application.Interfaces
{
    public interface IMessagePublisher
    {
        void Publish<T>(string exchange, string routingKey, T message) where T : class;
        void PublishToQueue<T>(string queueName, T message) where T : class;
    }
}
