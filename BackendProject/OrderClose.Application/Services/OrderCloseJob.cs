using ApiClient.Application.Endpoints;
using ApiClient.Application.Interfaces;
using OrderClose.Application.Interfaces;
using OrderClose.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClose.Application.Services
{
    public class OrderCloseJob : IOrderCloseJob
    {
        private readonly IApiClient _apiClient;

        public OrderCloseJob(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task ClosePendingOrders()
        {
            var orders = await _apiClient.GetDataAsync<List<OrderDto>>(ApiEndpoint.PendingOrders);

            foreach (var order in orders)
            {
                var closeUrl = ApiEndpoint.CloseOrder(order.Id);
                await _apiClient.PutDataAsync<object>(closeUrl, null);
            }
        }
    }
}
