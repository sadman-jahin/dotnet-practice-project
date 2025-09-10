using Application.Interfaces;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using Shared.Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly InventoryDbContext _context;

        public ProductRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductAsync(long id)
        {
            var product = await _context.Products.FindAsync(id);
            return ProductMapper.ToModel(product);
        }

        public async Task AddProductAsync(Product product)
        {
            var productEntity = ProductMapper.ToEntity(product);
            await _context.Products.AddAsync(productEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            var existingEntity = await _context.Products.FindAsync(product.Id);
            var updatedEntity = ProductMapper.MapToExistingEntity(product, existingEntity);
            _context.Products.Update(updatedEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsProductExists(List<long> ids)
        {
            return await _context.Products
                         .Where(product => ids.Contains(product.Id))
                         .CountAsync() == ids.Count;
        }

        public async Task DeductProductQuantityAsync(List<ProductDeductDto> products)
        {
            var productIds = products.Select(pr => pr.ProductId).ToList();

            var items = await _context.Products
                .Where(product => productIds.Contains(product.Id))
                .ToListAsync();

            foreach (var item in items)
            {
                var productToDeduct = products.FirstOrDefault(p => p.ProductId == item.Id);
                if (productToDeduct != null)
                {
                    item.Quantity -= productToDeduct.Quantity;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
