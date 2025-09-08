using Orders.Domain.Models;
using Orders.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Infrastructure.Mapper
{
    public static class OrderMapper
    {
        public static Order ToModel(OrderEntity entity)
        {
            if (entity == null)
                return null;

            return new Order
            {
                Id = entity.Id,
                CustomerEmail = entity.CustomerEmail,
                Status = entity.Status,
                Items = entity.Items?.Select(i => new OrderItem
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList() ?? new List<OrderItem>()
            };
        }

        public static OrderEntity ToEntity(Order model)
        {
            if (model == null)
                return null;

            return new OrderEntity
            {
                Id = model.Id,
                CustomerEmail = model.CustomerEmail,
                Status = model.Status,
                Items = model.Items?.Select(i => new OrderItemEntity
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList() ?? new List<OrderItemEntity>()
            };
        }

        public static void MapToExistingEntity(Order source, OrderEntity target)
        {
            target.CustomerEmail = source.CustomerEmail;
            target.Status = source.Status;

            // Replace the items if necessary
            target.Items = source.Items?.Select(i => new OrderItemEntity
            {
                Id = i.Id,
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList() ?? new List<OrderItemEntity>();
        }
    }
}