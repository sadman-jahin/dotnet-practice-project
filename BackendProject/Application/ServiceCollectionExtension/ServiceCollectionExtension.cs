using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Application.ServiceCollectionExtension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddProductServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProductService, ProductService>();
            return services;
        }
    }
}

