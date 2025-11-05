using ERP_API.DTOS.ReportStatistic;

namespace ERP_API.Repositories
{
    public interface IWarehouseReportRepository
    {
        Task<List<WarehouseStatisticDTO>> GetWarehouseStatisticsAsync(WarehouseStatisticRequestDTO request);
        Task<List<ProductStockDTO>> GetProductStockDetailAsync(WarehouseStatisticRequestDTO request);
        Task<PagedResultDTO<StockTransactionHistoryDTO>> GetStockHistoryAsync(StockHistoryRequestDTO request);
        Task<PagedResultDTO<CustomerOrderDTO>> GetCustomerOrdersAsync(CustomerOrderRequestDTO request);
        Task<CustomerOrderDTO> GetCustomerOrderDetailAsync(int orderId);

        Task<DashboardSummaryDTO> GetDashboardSummaryAsync();

    }
}