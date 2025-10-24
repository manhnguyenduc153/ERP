using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP_API.Repositories.tRepositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ErpDbContext _dbContext;

        public ProductRepository(ErpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateProductAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);

            if (product == null)
                return false;

            _dbContext.Products.Remove(product);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public Task<List<Product>> GetAllProductsAsync()
        {
            return _dbContext.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _dbContext.Products.FindAsync(productId);
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}