using Messaging.Application.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Application.Services
{
    public class RabbitMQService : IMessagingQueueSevice, IDisposable
    {
        private IConnection _connection;
        protected IChannel _channel;
        private string _queueName = "default_queue";

        public RabbitMQService()
        {
            InitializeConnection().GetAwaiter().GetResult();
        }

        private async Task InitializeConnection(string hostName = "localhost", string queueName = "default_queue")
        {
            _queueName = queueName;

            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = "guest",
                Password = "guest"
            };


            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync(); ;
            return;
        }

        public async Task DeclareQueueAsync(string queueName)
        {
            await _channel.QueueDeclareAsync(queue: queueName,
                                   durable: true,
                                   exclusive: false,
                                   autoDelete: false,
                                   arguments: null);
        }

        public async Task PublishAsync(string queueName, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message cannot be null or empty", nameof(message));

            await DeclareQueueAsync(queueName);

            var body = Encoding.UTF8.GetBytes(message);

            await _channel.BasicPublishAsync(
                 exchange: "",
                 routingKey: _queueName,
                 body: body
             );

            Console.WriteLine($" [x] Sent: {message}");
        }

        public async Task SubscribeAsync(string queueName, Func<string, Task> onMessageReceived)
        {
            await DeclareQueueAsync(queueName);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await onMessageReceived(message);
            };

            await _channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
        }


        public void Dispose()
        {
            _channel?.CloseAsync();
            _connection?.CloseAsync();
        }
    }
}