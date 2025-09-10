using Hangfire;
using Microsoft.AspNetCore.Builder;
using OrderClose.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClose.Application.ServiceCollectionExtension
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHangfire(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard();

            RecurringJob.AddOrUpdate<OrderCloseJob>("close-pending-orders", job => job.ClosePendingOrders(), Cron.Hourly);
            return app;
        }
    }
}
