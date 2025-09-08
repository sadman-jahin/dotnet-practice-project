
using Orders.Domain.Models;

namespace Orders.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(long id);
        Task<List<Order>> GetAllAsync();
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(long id);
    }

}
