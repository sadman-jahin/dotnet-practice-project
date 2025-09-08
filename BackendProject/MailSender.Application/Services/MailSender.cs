using Messaging.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Application.Services
{
    public class MailSender
    {
        private readonly IEmailQueueService _emailQueueService;
        public MailSender(IEmailQueueService emailQueueService) 
        { 
            _emailQueueService = emailQueueService;
        }
    }
}
