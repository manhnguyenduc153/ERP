using ERP_API.DTOS.ReportStatistic;
using ERP_API.Repositories;
using ERP_API.Services.IServices;

namespace ERP_API.Services
{
    public class WarehouseReportService : IWarehouseReportService
    {
        private readonly IWarehouseReportRepository _repository;

        public WarehouseReportService(IWarehouseReportRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<WarehouseStatisticDTO>> GetWarehouseStatisticsAsync(WarehouseStatisticRequestDTO request)
        {
            if (!request.FromDate.HasValue)
                request.FromDate = DateTime.Now.AddMonths(-1);

            if (!request.ToDate.HasValue)
                request.ToDate = DateTime.Now;

            var statistics = await _repository.GetWarehouseStatisticsAsync(request);
            return statistics;
        }

        public async Task<List<ProductStockDTO>> GetProductStockDetailAsync(WarehouseStatisticRequestDTO request)
        {
            var productStocks = await _repository.GetProductStockDetailAsync(request);
            return productStocks.OrderByDescending(x => x.TotalValue).ToList();
        }

        public async Task<PagedResultDTO<StockTransactionHistoryDTO>> GetStockHistoryAsync(StockHistoryRequestDTO request)
        {
            if (request.PageNumber < 1)
                request.PageNumber = 1;

            if (request.PageSize < 1 || request.PageSize > 100)
                request.PageSize = 20;

            return await _repository.GetStockHistoryAsync(request);
        }

        public async Task<PagedResultDTO<CustomerOrderDTO>> GetCustomerOrdersAsync(CustomerOrderRequestDTO request)
        {
            if (request.PageNumber < 1)
                request.PageNumber = 1;

            if (request.PageSize < 1 || request.PageSize > 100)
                request.PageSize = 20;

            return await _repository.GetCustomerOrdersAsync(request);
        }

        public async Task<CustomerOrderDTO> GetCustomerOrderDetailAsync(int orderId)
        {
            var order = await _repository.GetCustomerOrderDetailAsync(orderId);

            if (order == null)
                throw new KeyNotFoundException($"Không tìm thấy đơn hàng với ID {orderId}");

            return order;
        }
    }
}