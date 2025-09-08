using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Resources.Models
{
    public class EmailMessage
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }



    public class EmailMessageBuilder
    {
        private readonly EmailMessage _emailMessage;

        public EmailMessageBuilder()
        {
            _emailMessage = new EmailMessage();
        }

        public EmailMessageBuilder WithTo(string to)
        {
            _emailMessage.To = to;
            return this;
        }

        public EmailMessageBuilder WithSubject(string subject)
        {
            _emailMessage.Subject = subject;
            return this;
        }

        public EmailMessageBuilder WithBody(string body)
        {
            _emailMessage.Body = body;
            return this;
        }

        public EmailMessage Build()
        {
            // Optional: Add validation here if needed
            return _emailMessage;
        }
    }
}
