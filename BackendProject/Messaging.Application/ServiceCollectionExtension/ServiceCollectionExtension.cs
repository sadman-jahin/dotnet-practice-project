using Messaging.Application.Interfaces;
using Messaging.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Application.ServiceCollectionExtension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMessageQueueServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IMessagingQueueSevice, RabbitMQService>();
            services.AddSingleton<IEmailQueueService, EmailQueueService>();

            return services;
        }

    }
}
