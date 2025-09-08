using Messaging.Application.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Messaging.Application.Services
{
    public class EmailQueueService : RabbitMQService, IEmailQueueService
    {
        private readonly string _queueName = "email_queue";
        public async Task ProduceEmail(EmailMessage email)
        {
            if (string.IsNullOrWhiteSpace(email.Body))
                throw new ArgumentException("Message cannot be null or empty", nameof(email));
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(email));

            await DeclareQueueAsync(_queueName);

            await _channel.BasicPublishAsync(
                 exchange: "",
                 routingKey: _queueName,
                 body: body
             );
        }

        public async Task ConsumeEmail(Func<EmailMessage, Task> onMessageReceived)
        {
            await DeclareQueueAsync(_queueName);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var email = JsonSerializer.Deserialize<EmailMessage>(message);
                await onMessageReceived(email);
            };

            await _channel.BasicConsumeAsync(queue: _queueName, autoAck: true, consumer: consumer);
        }
    }
}
