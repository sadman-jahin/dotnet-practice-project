using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Mapper
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
    }

}
