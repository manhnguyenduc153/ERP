using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Core.Database.Entities;
using ERP_API.Repositories.tRepositories;
using ERP_API.Services.IServices;

namespace ERP_API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> CreateProductAsync(Product product)
        {
            return await _repository.CreateProductAsync(product);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            return await _repository.DeleteProductAsync(productId);
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _repository.GetAllProductsAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _repository.GetProductByIdAsync(productId);
        }

        public async Task<bool> UpdateProductAsync(int productId, Product product)
        {
            var existingProduct = await _repository.GetProductByIdAsync(productId);

            if (existingProduct == null)
            {
                return false;
            }

            existingProduct.ProductName = product.ProductName;
            existingProduct.UnitPrice = product.UnitPrice;
            existingProduct.Unit = product.Unit;
            existingProduct.CategoryId = product.CategoryId;

            return await _repository.UpdateProductAsync(existingProduct);
        }

    }
}