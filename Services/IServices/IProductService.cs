using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Entities;

namespace ERP_API.Services.IServices
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int productId);
        Task<bool> CreateProductAsync(Product product);
        Task<bool> UpdateProductAsync(int productId, Product product);
        Task<bool> DeleteProductAsync(int productId);
    }
}