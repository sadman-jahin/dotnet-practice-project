using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orders.Infrastructure.Data;
using Orders.Application.Interfaces;
using Orders.Infrastructure.Repositories;

namespace Orders.Infrastructure.ServiceCollectionExtension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddOrderModuleInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("OrderConnection")));


            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }

    }
}
