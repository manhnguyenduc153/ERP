using ERP_API.DTOS.PurchaseOrder;
using ERP_API.Entities;
using ERP_API.Mappers;
using ERP_API.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrdersController : ControllerBase
    {
        private readonly IPurchaseOrderService _service;
        private readonly ISupplierService _supplierService;
        private readonly IProductService _productService;

        public PurchaseOrdersController(IPurchaseOrderService service, ISupplierService supplierService, IProductService productService)
        {
            _service = service;
            _supplierService = supplierService;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchaseOrders()
        {
            var orders = await _service.GetListAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseOrderById(int id)
        {
            var order = await _service.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurchaseOrder([FromBody] CreatePurchaseOrderDTO orderDto)
        {

            if (orderDto.SupplierId <= 0)
            {
                // Handle new supplier creation logic here
                // For example, create a new Supplier entity and assign it to purchaseOrder.Supplier
                Supplier supplier = new Supplier
                {
                    SupplierName = orderDto.SupplierName,
                    Contact = orderDto.Contact
                };
                var rs = await _supplierService.CreareSupplierAsync(supplier);
                if (!rs)
                {
                    return BadRequest("Failed to create supplier.");
                }
                orderDto.SupplierId = supplier.SupplierId;
            }

            foreach (var purchaseDetail in orderDto.PurchaseOrderDetails)
            {
                if (purchaseDetail.ProductId <= 0)
                {
                    var product = new Product
                    {
                        ProductName = purchaseDetail.ProductName,
                        CategoryId = purchaseDetail.CategoryId,
                        UnitPrice = purchaseDetail.UnitPrice,
                    };
                    var rs = await _productService.CreateProductAsync(product);
                    if (!rs)
                    {
                        return BadRequest("Failed to create product.");
                    }
                    purchaseDetail.ProductId = product.ProductId;
                }
            }

            var orderPurchase = orderDto.ToEntity();

            var result = await _service.CreateAsync(orderPurchase);
            if (!result)
            {
                return StatusCode(500, "Failed to create purchase order.");
            }
            return Ok();
        }

    }
}
