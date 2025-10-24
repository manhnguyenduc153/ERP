using ERP_API.DTOS.ReportStatistic;
using ERP_API.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP_API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/warehouse-report")]
    public class WarehouseReportController : ControllerBase
    {
        private readonly ILogger<WarehouseReportController> _logger;
        private readonly IWarehouseReportService _warehouseReportService;

        public WarehouseReportController(
            ILogger<WarehouseReportController> logger,
            IWarehouseReportService warehouseReportService)
        {
            _logger = logger;
            _warehouseReportService = warehouseReportService;
        }


        [HttpGet("statistics")]
        public async Task<IActionResult> GetWarehouseStatistics([FromQuery] WarehouseStatisticRequestDTO request)
        {
            var result = await _warehouseReportService.GetWarehouseStatisticsAsync(request);
            return Ok(result);
        }

     
        [HttpGet("product-stock-detail")]
        public async Task<IActionResult> GetProductStockDetail([FromQuery] WarehouseStatisticRequestDTO request)
        {
            var result = await _warehouseReportService.GetProductStockDetailAsync(request);
            return Ok(result);
        }

        [HttpGet("stock-history")]
        public async Task<IActionResult> GetStockHistory([FromQuery] StockHistoryRequestDTO request)
        {
            var result = await _warehouseReportService.GetStockHistoryAsync(request);
            return Ok(result);
        }

        [HttpGet("customer-orders")]
        public async Task<IActionResult> GetCustomerOrders([FromQuery] CustomerOrderRequestDTO request)
        {
            var result = await _warehouseReportService.GetCustomerOrdersAsync(request);
            return Ok(result);
        }

        [HttpGet("customer-orders/{orderId}")]
        public async Task<IActionResult> GetCustomerOrderDetail(int orderId)
        {
            var result = await _warehouseReportService.GetCustomerOrderDetailAsync(orderId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}