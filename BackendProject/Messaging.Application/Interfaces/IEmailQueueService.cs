using Shared.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Application.Interfaces
{
    public interface IEmailQueueService : IMessagingQueueSevice
    {
        Task ProduceEmail(EmailMessage email);
        Task ConsumeEmail(Func<EmailMessage, Task> onMessageReceived);
    }
}
