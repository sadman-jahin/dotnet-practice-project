using Shared.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Application.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(EmailMessage message);
    }
}
