using ApiClient.Application.Interfaces;
using Messaging.Application.Interfaces;
using Messaging.Application.Services;
using Orders.Application.Dto;
using Orders.Application.Interfaces;
using Orders.Domain.Enum;
using Orders.Domain.Models;
using Shared.Resources.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiClient.Application.Endpoints;

namespace Orders.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEmailQueueService _emailProducer;
        private readonly IApiClient _apiClient;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepository,
            IEmailQueueService emailProducer,
            IApiClient apiClient,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _emailProducer = emailProducer;
            _apiClient = apiClient;
            _logger = logger;
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

        public async Task<List<Order>> GetPendingOrdersAsync()
        {
            return await _orderRepository.GetPendingOrders();
        }

        public async Task<bool> HasOrderValidProductIds(Order order)
        {
            var productIds = order.Items.Select(item => item.Id).ToList();
            var existsUrl = ApiEndpoint.ProductExists;

            try
            {
                return await _apiClient.PostDataAsync<bool>(existsUrl, productIds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating product IDs.");
                return false;
            }
        }

        public async Task CloseOrderAsync(long id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null || order.Status == OrderStatus.Closed)
            {
                _logger.LogWarning("Order {OrderId} is null or already closed.", id);
                return;
            }

            var productDeductions = GetProductDeductions(order);
            var isValidOrder = await ValidateProductQuantitiesAsync(productDeductions);

            if (isValidOrder)
            {
                await DeductProductQuantitiesAsync(productDeductions);
            }

            await NotifyCustomerAsync(order.CustomerEmail, isValidOrder);
            order.Status = OrderStatus.Closed;
            await _orderRepository.UpdateAsync(order);

            _logger.LogInformation("Order {OrderId} has been closed. Success: {IsValid}", id, isValidOrder);
        }

        private List<ProductDeductDto> GetProductDeductions(Order order)
        {
            return order.Items
                .GroupBy(item => item.ProductId)
                .Select(g => new ProductDeductDto
                {
                    ProductId = g.Key,
                    Quantity = g.Sum(item => item.Quantity)
                })
                .ToList();
        }

        private async Task<bool> ValidateProductQuantitiesAsync(List<ProductDeductDto> deductions)
        {
            foreach (var item in deductions)
            {
                try
                {
                    var productUrl = ApiEndpoint.ProductById(item.ProductId);
                    var product = await _apiClient.GetDataAsync<ProductDto>(productUrl);

                    if (product.Quantity < item.Quantity)
                    {
                        _logger.LogWarning("Product {ProductId} has insufficient quantity.", item.ProductId);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error fetching product {ProductId} for quantity validation.", item.ProductId);
                    return false;
                }
            }

            return true;
        }

        private async Task DeductProductQuantitiesAsync(List<ProductDeductDto> deductions)
        {
            try
            {
                var deductUrl = ApiEndpoint.ProductDeduct;
                await _apiClient.PutDataAsync<string>(deductUrl, deductions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deducting product quantities.");
                throw;
            }
        }

        private async Task NotifyCustomerAsync(string email, bool success)
        {
            var message = new EmailMessageBuilder()
                            .WithTo(email)
                            .WithSubject("Order Result")
                            .WithBody(success
                                ? "Your order has been successfully processed."
                                : "Your order failed due to insufficient product quantity.")
                            .Build();

            try
            {
                await _emailProducer.ProduceEmail(message);
                _logger.LogInformation("Email sent to {CustomerEmail}.", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send order result email to {CustomerEmail}.", email);
            }
        }
    }
}
