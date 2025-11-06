using ERP_API.DTOS.ReportStatistic;
using ERP_API.Core.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP_API.Repositories
{
    public class WarehouseReportRepository : IWarehouseReportRepository
    {
        private readonly ErpDbContext _context;

        public WarehouseReportRepository(ErpDbContext context)
        {
            _context = context;
        }
        public async Task<List<WarehouseStatisticDTO>> GetWarehouseStatisticsAsync(WarehouseStatisticRequestDTO request)
        {
            // THÊM .Include(x => x.Warehouse) VÀO ĐÂY
            var query = _context.StockTransactions
                .Include(x => x.Warehouse)  // ← FIX: THÊM DÒNG NÀY
                .AsQueryable();

            if (request.WarehouseId.HasValue)
                query = query.Where(x => x.WarehouseId == request.WarehouseId.Value);

            if (request.FromDate.HasValue)
                query = query.Where(x => x.TransactionDate >= request.FromDate.Value);

            if (request.ToDate.HasValue)
                query = query.Where(x => x.TransactionDate <= request.ToDate.Value);

            if (request.ProductId.HasValue)
                query = query.Where(x => x.ProductId == request.ProductId.Value);

            var statistics = await query
                .GroupBy(x => new { x.WarehouseId, x.Warehouse.WarehouseName, x.Warehouse.Location })
                .Select(g => new WarehouseStatisticDTO
                {
                    WarehouseId = g.Key.WarehouseId ?? 0,
                    WarehouseName = g.Key.WarehouseName ?? "",  // Đơn giản hóa
                    Location = g.Key.Location ?? "",
                    TotalImport = g.Where(x => x.TransactionType == "IMPORT").Sum(x => x.Quantity ?? 0),
                    TotalExport = g.Where(x => x.TransactionType == "EXPORT").Sum(x => x.Quantity ?? 0),
                    DamagedItems = g.Where(x => x.TransactionType == "DAMAGED").Sum(x => x.Quantity ?? 0),
                    CurrentStock = g.Where(x => x.TransactionType == "IMPORT").Sum(x => x.Quantity ?? 0) -
                                 g.Where(x => x.TransactionType == "EXPORT").Sum(x => x.Quantity ?? 0) -
                                 g.Where(x => x.TransactionType == "DAMAGED").Sum(x => x.Quantity ?? 0),
                    FromDate = request.FromDate ?? DateTime.MinValue,
                    ToDate = request.ToDate ?? DateTime.MaxValue
                })
                .ToListAsync();

            return statistics;
        }
        public async Task<List<ProductStockDTO>> GetProductStockDetailAsync(WarehouseStatisticRequestDTO request)
        {
            var query = _context.StockTransactions
                .Include(x => x.Product)
                .Include(x => x.Warehouse)
                .AsQueryable();

            if (request.WarehouseId.HasValue)
                query = query.Where(x => x.WarehouseId == request.WarehouseId.Value);

            if (request.ProductId.HasValue)
                query = query.Where(x => x.ProductId == request.ProductId.Value);

            if (request.FromDate.HasValue)
                query = query.Where(x => x.TransactionDate >= request.FromDate.Value);

            if (request.ToDate.HasValue)
                query = query.Where(x => x.TransactionDate <= request.ToDate.Value);

            var productStocks = await query
                .GroupBy(x => new
                {
                    x.ProductId,
                    ProductName = x.Product.ProductName,
                    x.WarehouseId,
                    WarehouseName = x.Warehouse.WarehouseName,
                    UnitPrice = x.Product.UnitPrice
                })
                .Select(g => new ProductStockDTO
                {
                    ProductId = g.Key.ProductId ?? 0,
                    ProductName = g.Key.ProductName == null ? "" : g.Key.ProductName,
                    WarehouseId = g.Key.WarehouseId ?? 0,
                    WarehouseName = g.Key.WarehouseName == null ? "" : g.Key.WarehouseName,
                    QuantityImport = g.Where(x => x.TransactionType == "IMPORT").Sum(x => x.Quantity ?? 0),
                    QuantityExport = g.Where(x => x.TransactionType == "EXPORT").Sum(x => x.Quantity ?? 0),
                    DamagedQuantity = g.Where(x => x.TransactionType == "DAMAGED").Sum(x => x.Quantity ?? 0),
                    CurrentStock = g.Where(x => x.TransactionType == "IMPORT").Sum(x => x.Quantity ?? 0) -
                                 g.Where(x => x.TransactionType == "EXPORT").Sum(x => x.Quantity ?? 0) -
                                 g.Where(x => x.TransactionType == "DAMAGED").Sum(x => x.Quantity ?? 0),
                    TotalValue = (g.Where(x => x.TransactionType == "IMPORT").Sum(x => x.Quantity ?? 0) -
                                g.Where(x => x.TransactionType == "EXPORT").Sum(x => x.Quantity ?? 0) -
                                g.Where(x => x.TransactionType == "DAMAGED").Sum(x => x.Quantity ?? 0)) * (g.Key.UnitPrice ?? 0)
                })
                .ToListAsync();

            return productStocks;
        }
        public async Task<PagedResultDTO<StockTransactionHistoryDTO>> GetStockHistoryAsync(StockHistoryRequestDTO request)
        {
            request.TransactionType ??= "ALL";

            var query = _context.StockTransactions
                .Include(x => x.Product)
                .Include(x => x.Warehouse)
                .AsQueryable();

            if (request.WarehouseId.HasValue)
                query = query.Where(x => x.WarehouseId == request.WarehouseId.Value);

            if (request.ProductId.HasValue)
                query = query.Where(x => x.ProductId == request.ProductId.Value);

            query = query.Where(x => x.TransactionType == "IMPORT" || x.TransactionType == "EXPORT");

            if (!string.IsNullOrEmpty(request.TransactionType) && request.TransactionType != "ALL")
                query = query.Where(x => x.TransactionType == request.TransactionType);

            if (request.FromDate.HasValue)
                query = query.Where(x => x.TransactionDate >= request.FromDate.Value);

            if (request.ToDate.HasValue)
                query = query.Where(x => x.TransactionDate <= request.ToDate.Value);

            var totalRecords = await query.CountAsync();

            var stockTransactions = await query
                .OrderByDescending(x => x.TransactionDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var result = new List<StockTransactionHistoryDTO>();

            foreach (var trans in stockTransactions)
            {
                decimal unitPrice = 0;

                if (trans.TransactionType == "IMPORT")
                {
                    var poDetail = await _context.PurchaseOrderDetails
                       .Where(x => x.ProductId == trans.ProductId)
                       .OrderByDescending(x => x.PurchaseOrder.OrderDate)
                       .FirstOrDefaultAsync();

                    if (poDetail != null)
                        unitPrice = poDetail.UnitPrice ?? 0;
                }
                else if (trans.TransactionType == "EXPORT")
                {
                    var soDetail = await _context.SalesOrderDetails
                           .Where(x => x.ProductId == trans.ProductId)
                           .OrderByDescending(x => x.SalesOrder.OrderDate)
                           .FirstOrDefaultAsync();

                    if (soDetail != null)
                        unitPrice = soDetail.UnitPrice ?? 0;
                }

                result.Add(new StockTransactionHistoryDTO
                {
                    TransactionId = trans.TransactionId,
                    ProductId = trans.ProductId ?? 0,
                    ProductName = trans.Product?.ProductName ?? "",
                    TransactionType = trans.TransactionType ?? "",
                    Quantity = trans.Quantity ?? 0,
                    TransactionDate = trans.TransactionDate ?? DateTime.Now,
                    WarehouseId = trans.WarehouseId ?? 0,
                    WarehouseName = trans.Warehouse?.WarehouseName ?? "",
                    UnitPrice = unitPrice,
                    TotalValue = (trans.Quantity ?? 0) * unitPrice
                });
            }

            return new PagedResultDTO<StockTransactionHistoryDTO>
            {
                Data = result,
                TotalRecords = totalRecords,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }
        public async Task<PagedResultDTO<CustomerOrderDTO>> GetCustomerOrdersAsync(CustomerOrderRequestDTO request)
        {
            var query = _context.SalesOrders
                .Include(x => x.Customer)
                .Include(x => x.SalesOrderDetails)
                .ThenInclude(d => d.Product)
                .AsQueryable();

            if (request.CustomerId.HasValue)
                query = query.Where(x => x.CustomerId == request.CustomerId.Value);

            if (!string.IsNullOrEmpty(request.Status))
                query = query.Where(x => x.Status == request.Status);

            if (request.FromDate.HasValue)
                query = query.Where(x => x.OrderDate.HasValue && x.OrderDate.Value >= request.FromDate.Value);

            if (request.ToDate.HasValue)
                query = query.Where(x => x.OrderDate.HasValue && x.OrderDate.Value <= request.ToDate.Value);

            var totalRecords = await query.CountAsync();

            var orders = await query
                .OrderByDescending(x => x.OrderDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CustomerOrderDTO
                {
                    SalesOrderId = x.SalesOrderId,
                    OrderDate = x.OrderDate.HasValue ? x.OrderDate.Value.ToString("yyyy-MM-dd") : "",
                    CustomerId = x.CustomerId ?? 0,
                    CustomerName = x.Customer != null ? (x.Customer.Name == null ? "" : x.Customer.Name) : "",
                    Status = x.Status == null ? "" : x.Status,
                    TotalAmount = x.SalesOrderDetails.Sum(d => (d.Quantity * d.UnitPrice) ?? 0),
                    OrderDetails = x.SalesOrderDetails.Select(d => new CustomerOrderDetailDTO
                    {
                        DetailId = d.DetailId,
                        ProductId = d.ProductId ?? 0,
                        ProductName = d.Product != null ? (d.Product.ProductName == null ? "" : d.Product.ProductName) : "",
                        Quantity = d.Quantity ?? 0,
                        UnitPrice = d.UnitPrice ?? 0,
                        TotalPrice = (d.Quantity ?? 0) * (d.UnitPrice ?? 0)
                    }).ToList()
                })
                .ToListAsync();

            return new PagedResultDTO<CustomerOrderDTO>
            {
                Data = orders,
                TotalRecords = totalRecords,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<CustomerOrderDTO> GetCustomerOrderDetailAsync(int orderId)
        {
            var order = await _context.SalesOrders
                .Include(x => x.Customer)
                .Include(x => x.SalesOrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(x => x.SalesOrderId == orderId);

            if (order == null)
                return null;

            return new CustomerOrderDTO
            {
                SalesOrderId = order.SalesOrderId,
                OrderDate = order.OrderDate.HasValue ? order.OrderDate.Value.ToString("yyyy-MM-dd") : "",
                CustomerId = order.CustomerId ?? 0,
                CustomerName = order.Customer != null ? (order.Customer.Name == null ? "" : order.Customer.Name) : "",
                Status = order.Status == null ? "" : order.Status,
                TotalAmount = order.SalesOrderDetails.Sum(d => (d.Quantity * d.UnitPrice) ?? 0),
                OrderDetails = order.SalesOrderDetails?.Select(d => new CustomerOrderDetailDTO
                {
                    DetailId = d.DetailId,
                    ProductId = d.ProductId ?? 0,
                    ProductName = d.Product != null ? (d.Product.ProductName == null ? "" : d.Product.ProductName) : "",
                    Quantity = d.Quantity ?? 0,
                    UnitPrice = d.UnitPrice ?? 0,
                    TotalPrice = (d.Quantity ?? 0) * (d.UnitPrice ?? 0)
                }).ToList() ?? new List<CustomerOrderDetailDTO>()
            };
        }
        public async Task<DashboardSummaryDTO> GetDashboardSummaryAsync()
        {
            var totalWarehouses = await _context.Warehouses.CountAsync();

            var totalProducts = await _context.Products.CountAsync();

            var totalCustomers = await _context.Customers.CountAsync();

            var totalOrders = await _context.SalesOrders.CountAsync();

            var pendingOrders = await _context.SalesOrders
                .Where(x => x.Status == "Pending")
                .CountAsync();

            var inventoryData = await _context.StockTransactions
                .Include(x => x.Product)
                .GroupBy(x => new { x.ProductId, x.Product.UnitPrice })
                .Select(g => new
                {
                    ProductId = g.Key.ProductId,
                    Stock = g.Where(x => x.TransactionType == "IMPORT").Sum(x => x.Quantity ?? 0) -
                           g.Where(x => x.TransactionType == "EXPORT").Sum(x => x.Quantity ?? 0) -
                           g.Where(x => x.TransactionType == "DAMAGED").Sum(x => x.Quantity ?? 0),
                    UnitPrice = g.Key.UnitPrice
                })
                .ToListAsync();

            var totalInventoryValue = inventoryData
                .Sum(x => (decimal)(x.Stock * (x.UnitPrice ?? 0)));

            return new DashboardSummaryDTO
            {
                TotalWarehouses = totalWarehouses,
                TotalProducts = totalProducts,
                TotalCustomers = totalCustomers,
                TotalOrders = totalOrders,
                PendingOrders = pendingOrders,
                TotalInventoryValue = totalInventoryValue
            };
        }
    }
}