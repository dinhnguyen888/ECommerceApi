namespace ECommerce.AuthService.Application.Interfaces
{
    public interface IMessageConsumer
    {
        void Consume<T>(string queueName, Action<T> onMessage) where T : class;
    }
}
