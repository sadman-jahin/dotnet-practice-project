using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetProduct(long id);
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(long id);
    }
}
