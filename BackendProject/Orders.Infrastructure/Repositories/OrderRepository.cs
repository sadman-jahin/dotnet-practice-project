using Microsoft.EntityFrameworkCore;
using Orders.Application.Interfaces;
using Orders.Domain.Enum;
using Orders.Domain.Models;
using Orders.Infrastructure.Data;
using Orders.Infrastructure.Entities;
using Orders.Infrastructure.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var entity = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);

            return OrderMapper.ToModel(entity);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            var entities = await _context.Orders
                .Include(o => o.Items)
                .ToListAsync();

            return entities.Select(OrderMapper.ToModel).ToList();
        }

        public async Task AddAsync(Order order)
        {
            var entity = OrderMapper.ToEntity(order);
            await _context.Orders.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            var existingEntity = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            if (existingEntity != null)
            {
                OrderMapper.MapToExistingEntity(order, existingEntity);
                _context.Orders.Update(existingEntity);
                await _context.SaveChangesAsync();
            }
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

        public async Task<List<Order>> GetPendingOrders()
        {
            var orders = await _context.Orders.Where(order => order.Status == OrderStatus.Pending).ToListAsync();
            return orders.Select(OrderMapper.ToModel).ToList();
        }
    }
}
