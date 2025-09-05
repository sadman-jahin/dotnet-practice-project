using Domain.Models;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mapper
{
    public static class ProductMapper
    {
        public static Product ToModel(ProductEntity entity)
        {
            if (entity == null)
                return null;

            return new Product
            {
                Id = entity.Id,
                Name = entity.Name,
                Quantity = entity.Quantity
            };
        }

        public static ProductEntity ToEntity(Product product)
        {
            if (product == null)
                return null;

            return new ProductEntity
            {
                Id = product.Id,
                Name = product.Name,
                Quantity = product.Quantity
            };
        }

        public static ProductEntity MapToExistingEntity(Product source, ProductEntity target)
        {
            target.Name = source.Name;
            target.Quantity = source.Quantity;
            return target;
        }

    }

}
