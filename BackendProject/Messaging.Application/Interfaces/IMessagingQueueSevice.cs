using Shared.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Application.Interfaces
{
    public interface IMessagingQueueSevice
    {
        Task PublishAsync(string queueName, string message);
        Task SubscribeAsync(string queueName, Func<string, Task> onMessageReceived);
    }
}
