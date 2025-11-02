using System.Text;
using System.Text.Json;
using ECommerce.TransactionService.Application.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ECommerce.TransactionService.Infrastructure.MessageBroker
{
    public class RabbitMqConsumer : IMessageConsumer, IDisposable
    {
        private readonly RabbitMqConnection _connection;
        private readonly IModel _channel;
        private readonly List<string> _consumerTags = new();

        public RabbitMqConsumer(RabbitMqConnection connection)
        {
            _connection = connection;
            _channel = _connection.Channel;
        }

        public void Consume<T>(string queueName, Action<T> onMessage) where T : class
        {
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var deserialized = JsonSerializer.Deserialize<T>(message);

                if (deserialized != null)
                {
                    onMessage(deserialized);
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            };

            var consumerTag = _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
            _consumerTags.Add(consumerTag);
        }

        public void Dispose()
        {
            foreach (var tag in _consumerTags)
            {
                _channel?.BasicCancel(tag);
            }
            _channel?.Close();
            _channel?.Dispose();
        }
    }
}
