using Orders.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Order> GetOrderByIdAsync(long id);
        Task<List<Order>> GetAllOrdersAsync();
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(long id);
        Task CloseOrderAsync(long id);
        Task<List<Order>> GetPendingOrdersAsync();
        Task<bool> HasOrderValidProductIds(Order order);
    }
}
