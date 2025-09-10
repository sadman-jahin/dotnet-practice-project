using Domain.Models;
using Shared.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductAsync(long id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(long id);
        Task<bool> IsProductExists(List<long> ids);
        Task DeductProductQuantityAsync(List<ProductDeductDto> products);
    }
}
