using System.Text;
using System.Text.Json;
using ECommerce.AuthService.Application.Interfaces;
using RabbitMQ.Client;

namespace ECommerce.AuthService.Infrastructure.MessageBroker
{
    public class RabbitMqPublisher : IMessagePublisher, IDisposable
    {
        private readonly RabbitMqConnection _connection;
        private readonly IModel _channel;

        public RabbitMqPublisher(RabbitMqConnection connection)
        {
            _connection = connection;
            _channel = _connection.Channel;
        }

        public void Publish<T>(string exchange, string routingKey, T message) where T : class
        {
            _channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: properties, body: body);
        }

        public void PublishToQueue<T>(string queueName, T message) where T : class
        {
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: properties, body: body);
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
        }
    }
}
