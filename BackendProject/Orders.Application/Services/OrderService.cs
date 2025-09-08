using ApiClient.Application.Interfaces;
using Messaging.Application.Interfaces;
using Messaging.Application.Services;
using Orders.Application.Dto;
using Orders.Application.Interfaces;
using Orders.Domain.Enum;
using Orders.Domain.Models;
using Shared.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEmailQueueService _emailProducer;
        private readonly IApiClient _apiClient;

        public OrderService(IOrderRepository orderRepository, IEmailQueueService emailProducer, IApiClient apiClient)
        {
            _orderRepository = orderRepository;
            _emailProducer = emailProducer;
            _apiClient = apiClient;
        }

        public async Task<Order> GetOrderByIdAsync(long id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task AddOrderAsync(Order order)
        {
            await _orderRepository.AddAsync(order);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteOrderAsync(long id)
        {
            await _orderRepository.DeleteAsync(id);
        }

        public async Task CloseOrderAsync(long id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return;
            }
            bool isValidOrder = true;
            foreach (var item in order.Items)
            {
                var requestUrl = $"https://localhost:44322/api/v1/product/{item.Id}";
                var product = await _apiClient.GetDataAsync<ProductDto>(requestUrl);
                if (product.Quantity >= item.Quantity)
                {
                    continue;
                }
                else
                {
                    isValidOrder = false;
                }
            }

            var email = new EmailMessageBuilder()
                                .WithTo(order.CustomerEmail)
                                .WithSubject("Order Result")
                                .WithBody(isValidOrder ? "Your order is successfully processed." : "Your order is failed due to insufficient quantity.")
                                .Build();

            await _emailProducer.ProduceEmail(email);

            if (isValidOrder)
            {
                //order.Status = OrderStatus.Closed;
                //await _orderRepository.UpdateAsync(order);
            }
            return;
        }

    }

}
