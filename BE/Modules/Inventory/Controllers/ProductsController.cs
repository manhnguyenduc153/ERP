using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.DTOS.Product;
using ERP_API.Mappers;
using ERP_API.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IAuditLogService _auditLogService;

        public ProductsController(IProductService service, IAuditLogService auditLogService)
        {
            _service = service;
            _auditLogService = auditLogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _service.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _service.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO createProductDTO)
        {
            var product = createProductDTO.ToEntity();

            // Ghi log - Service sẽ tự động kiểm tra Success/Failed
            await _auditLogService.LogAsync(
                action: "CREATE_PRODUCT",
                endpoint: "/api/products",
                oldValue: null,
                newValue: product
            );

            var result = await _service.CreateProductAsync(product);
            if (result)
            {
                return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
            }

            return StatusCode(500, "A problem happened while handling your request.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductDTO updateProductDTO)
        {
            // Lấy dữ liệu cũ trước khi update
            var oldProduct = await _service.GetProductByIdAsync(updateProductDTO.ProductId);
            var product = updateProductDTO.ToEntity();
            
            // Ghi log - Service sẽ tự động kiểm tra Success/Failed
            await _auditLogService.LogAsync(
                action: "UPDATE_PRODUCT",
                endpoint: $"/api/products/{updateProductDTO.ProductId}",
                oldValue: oldProduct,
                newValue: product
            );
            
            var result = await _service.UpdateProductAsync(updateProductDTO.ProductId, product);
            if (result)
            {
                return NoContent();
            }
            return StatusCode(500, "A problem happened while handling your request.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // Lấy dữ liệu trước khi xóa
            var product = await _service.GetProductByIdAsync(id);
            
            // Ghi log - Service sẽ tự động kiểm tra Success/Failed
            await _auditLogService.LogAsync(
                action: "DELETE_PRODUCT",
                endpoint: $"/api/products/{id}",
                oldValue: product,
                newValue: null
            );
            
            var result = await _service.DeleteProductAsync(id);
            if (result)
            {
                return NoContent();
            }
            return StatusCode(500, "A problem happened while handling your request.");
        }

    }
}