using Application.Interfaces;
using Domain.Models;
using Shared.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Product> GetProduct(long id)
        {
            return await _repository.GetProductAsync(id);
        }

        public async Task AddProduct(Product product)
        {
            await _repository.AddProductAsync(product);
        }

        public async Task UpdateProduct(Product product)
        {
            await _repository.UpdateProductAsync(product);
        }

        public async Task DeleteProduct(long id)
        {
            await _repository.DeleteProductAsync(id);
        }

        public async Task<bool> IsProductExists(List<long> ids)
        {
            return await _repository.IsProductExists(ids);
        }

        public async Task DeductProductQuantityAsync(List<ProductDeductDto> products)
        {
            await _repository.DeductProductQuantityAsync(products);
        }
    }
}
