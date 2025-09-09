using MailKit.Net.Smtp;
using MailKit.Security;
using MailSender.Application.Interfaces;
using Messaging.Application.Interfaces;
using Microsoft.Extensions.Hosting;
using MimeKit;
using Shared.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Application.Services
{
    public class MailService : IMailService, IHostedService
    {
        private readonly IEmailQueueService _emailQueueService;
        private readonly EmailConfiguration _emailConfiguration;
        public MailService(IEmailQueueService emailQueueService, EmailConfiguration emailConfiguration)
        {
            _emailQueueService = emailQueueService;
            _emailConfiguration = emailConfiguration;
        }

        private async Task InitiateSubscription()
        {
            await _emailQueueService.ConsumeEmail(async (emailMessage) =>
            {
                await SendEmailAsync(emailMessage);
            });
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfiguration.Host, _emailConfiguration.From));
            emailMessage.To.AddRange(message.To.Select(x => new MailboxAddress("email", x)));
            emailMessage.Subject = message.Subject;
            var contentType = message.Type == Shared.Resources.Models.ContentType.HTML ? MimeKit.Text.TextFormat.Html : MimeKit.Text.TextFormat.Text;
            emailMessage.Body = new TextPart(contentType) { Text = message.Body };
            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                // Only for dev/test — will be remove in production!
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
                await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, SecureSocketOptions.Auto);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password);
                await client.SendAsync(mailMessage);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to send email", ex);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await InitiateSubscription();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
