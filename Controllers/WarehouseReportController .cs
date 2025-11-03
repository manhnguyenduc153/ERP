using ERP_API.DTOS.ReportStatistic;
using ERP_API.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP_API.Controllers
{
    [ApiController]
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
            var response = new
            {
                success = true,
                message = "Warehouse statistics retrieved successfully",
                data = result
            };
            return Ok(response);
        }

        [HttpGet("product-stock-detail")]
        public async Task<IActionResult> GetProductStockDetail([FromQuery] WarehouseStatisticRequestDTO request)
        {
            var result = await _warehouseReportService.GetProductStockDetailAsync(request);
            var response = new
            {
                success = true,
                message = "Product stock details retrieved successfully",
                data = result
            };
            return Ok(response);
        }

        [HttpGet("stock-history")]
        public async Task<IActionResult> GetStockHistory([FromQuery] StockHistoryRequestDTO request)
        {
            var result = await _warehouseReportService.GetStockHistoryAsync(request);

            var response = new
            {
                success = true,
                message = "Stock history retrieved successfully",
                data = result.Data,
                metaData = new
                {
                    totalItems = result.TotalRecords,
                    pageIndex = result.PageNumber,
                    pageSize = result.PageSize,
                    totalPages = result.TotalPages,
                    hasNext = result.HasNext,
                    hasPrevious = result.HasPrevious
                }
            };

            return Ok(response);
        }

        [HttpGet("customer-orders")]
        public async Task<IActionResult> GetCustomerOrders([FromQuery] CustomerOrderRequestDTO request)
        {
            var result = await _warehouseReportService.GetCustomerOrdersAsync(request);
            var response = new
            {
                success = true,
                message = "Customer orders retrieved successfully",
                data = result.Data,
                metaData = new
                {
                    totalItems = result.TotalRecords,
                    pageIndex = result.PageNumber,
                    pageSize = result.PageSize,
                    totalPages = result.TotalPages,
                    hasNext = result.HasNext,
                    hasPrevious = result.HasPrevious
                }
            };
            return Ok(response);
        }

        [HttpGet("customer-orders/{orderId}")]
        public async Task<IActionResult> GetCustomerOrderDetail(int orderId)
        {
            var result = await _warehouseReportService.GetCustomerOrderDetailAsync(orderId);
            if (result == null)
            {
                return NotFound(new { success = false, message = "Order not found" });
            }
            var response = new
            {
                success = true,
                message = "Order details retrieved successfully",
                data = result
            };
            return Ok(response);
        }
        [HttpGet("dashboard-summary")]
public async Task<IActionResult> GetDashboardSummary()
{
  
        var result = await _warehouseReportService.GetDashboardSummaryAsync();
        var response = new
        {
            success = true,
            message = "Dashboard summary retrieved successfully",
            data = result
        };
        return Ok(response);
  
}
    }
}