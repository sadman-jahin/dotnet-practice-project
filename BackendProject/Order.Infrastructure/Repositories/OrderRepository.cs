using Orders.Infrastructure.Data;
using Orders.Infrastructure.Entities;
using Orders.Application.Interfaces;
using Orders.Domain.Models;

namespace Orders.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetByIdAsync(long id)
        {
            var entity = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
            return entity == null ? null : MapToDomain(entity);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            var entities = await _context.Orders.Include(o => o.Items).ToListAsync();
            return entities.Select(MapToDomain).ToList();
        }

        public async Task AddAsync(Order order)
        {
            var entity = MapToEntity(order);
            await _context.Orders.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            var entity = MapToEntity(order);
            _context.Orders.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _context.Orders.FindAsync(id);
            if (entity != null)
            {
                _context.Orders.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        // Mapping Helpers
        private Order MapToDomain(OrderEntity entity)
        {
            return new Order
            {
                Id = entity.Id,
                OrderDate = entity.OrderDate,
                CustomerName = entity.CustomerName,
                TotalAmount = entity.TotalAmount,
                Items = entity.Items.Select(i => new OrderItem
                {
                    Id = i.Id,
                    Name = long.TryParse(i.Name, out var nameId) ? nameId : 0
                }).ToList()
            };
        }

        private OrderEntity MapToEntity(Order order)
        {
            return new OrderEntity
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                CustomerName = order.CustomerName,
                TotalAmount = order.TotalAmount,
                Items = order.Items.Select(i => new OrderItemEntity
                {
                    Id = i.Id,
                    Name = i.Name.ToString()
                }).ToList()
            };
        }
    }

}
