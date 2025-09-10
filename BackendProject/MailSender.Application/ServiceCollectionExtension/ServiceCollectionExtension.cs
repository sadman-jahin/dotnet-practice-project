using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Resources.Models;
using MailSender.Application.Interfaces;
using MailSender.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSender.Application.ServiceCollectionExtension
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEmailServices(this IServiceCollection services, IConfiguration configuration)
        {
            var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddHostedService<MailService>();
        }
    }
}
