using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Resources.Models
{
    public class EmailMessage
    {
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public ContentType Type { get; set; } = ContentType.Plain;
    }
    public enum ContentType
    {
        Plain = 1,
        HTML = 2
    }

    public class EmailMessageBuilder
    {
        private readonly EmailMessage _emailMessage;

        public EmailMessageBuilder()
        {
            _emailMessage = new EmailMessage
            {
                To = new List<string>()
            };
        }

        public EmailMessageBuilder WithTo(string to)
        {
            if (!string.IsNullOrWhiteSpace(to))
            {
                _emailMessage.To.Add(to);
            }
            return this;
        }

        public EmailMessageBuilder WithTo(List<string> to)
        {
            if (to != null && to.Any())
            {
                _emailMessage.To.AddRange(to);
            }
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
            if (_emailMessage.To == null || !_emailMessage.To.Any())
            {
                throw new InvalidOperationException("The 'To' list cannot be empty.");
            }

            if (string.IsNullOrEmpty(_emailMessage.Subject))
            {
                throw new InvalidOperationException("Subject cannot be empty.");
            }

            if (string.IsNullOrEmpty(_emailMessage.Body))
            {
                throw new InvalidOperationException("Body cannot be empty.");
            }

            return _emailMessage;
        }
    }
}
