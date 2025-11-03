using System;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ECommerce.TransactionService.Application.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ECommerce.TransactionService.Infrastructure.MessageBroker
{
    // Service xu ly Request-Reply pattern
    public class RabbitMqRequestReplyService : IRequestReplyService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _replyQueueName;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> _pendingRequests;
        private readonly EventingBasicConsumer _consumer;
        private bool _disposed = false;

        public RabbitMqRequestReplyService(RabbitMqConnection connection)
        {
            // Tao channel rieng cho Request-Reply service de tranh conflict
            _connection = connection.Connection;
            _channel = _connection.CreateModel();
            _pendingRequests = new ConcurrentDictionary<string, TaskCompletionSource<string>>();

            // Tao reply queue (temporary queue, se tu dong xoa khi disconnect)
            _replyQueueName = _channel.QueueDeclare(exclusive: true).QueueName;

            // Setup consumer de nhan response
            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (model, ea) =>
            {
                var correlationId = ea.BasicProperties.CorrelationId;

                if (!string.IsNullOrEmpty(correlationId) && _pendingRequests.TryRemove(correlationId, out var tcs))
                {
                    var body = ea.Body.ToArray();
                    var response = Encoding.UTF8.GetString(body);
                    tcs.SetResult(response);
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: _replyQueueName, autoAck: false, consumer: _consumer);
        }

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(
            string queueName,
            TRequest request,
            TimeSpan? timeout = null) where TRequest : class where TResponse : class
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RabbitMqRequestReplyService));

            if (_channel == null || _channel.IsClosed)
                throw new InvalidOperationException("RabbitMQ channel is closed or disposed");

            timeout ??= TimeSpan.FromSeconds(30); // Default timeout 30 seconds

            // Tao correlation ID
            var correlationId = Guid.NewGuid().ToString();

            // Tao task completion source de cho response
            var tcs = new TaskCompletionSource<string>();
            _pendingRequests.TryAdd(correlationId, tcs);

            try
            {
                // Declare request queue
                _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                // Serialize request
                var json = JsonSerializer.Serialize(request);
                var body = Encoding.UTF8.GetBytes(json);

                // Tao properties
                var properties = _channel.CreateBasicProperties();
                properties.CorrelationId = correlationId;
                properties.ReplyTo = _replyQueueName;
                properties.Persistent = true;

                // Gui request
                _channel.BasicPublish(
                    exchange: "",
                    routingKey: queueName,
                    basicProperties: properties,
                    body: body);

                // Cho response voi timeout
                using var cts = new CancellationTokenSource(timeout.Value);
                cts.Token.Register(() => tcs.TrySetCanceled());

                var responseJson = await tcs.Task;

                // Deserialize response
                var response = JsonSerializer.Deserialize<TResponse>(responseJson);
                if (response == null)
                {
                    throw new InvalidOperationException("Khong the deserialize response");
                }

                return response;
            }
            catch (TaskCanceledException)
            {
                _pendingRequests.TryRemove(correlationId, out _);
                throw new TimeoutException($"Request timeout sau {timeout.Value.TotalSeconds} giay");
            }
            catch (Exception)
            {
                _pendingRequests.TryRemove(correlationId, out _);
                throw;
            }
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            try
            {
                _consumer?.Model?.Close();
            }
            catch { }

            try
            {
                _channel?.Close();
                _channel?.Dispose();
            }
            catch { }

            _disposed = true;
        }
    }
}

