using ERP_API.Core.Database.Entities;
using ERP_API.Repositories.IRepositories;
using ERP_API.Services.IServices;

namespace ERP_API.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _repository;
        private readonly IStockTransactionService _stockTransactionService;

        public PurchaseOrderService(IPurchaseOrderRepository repository, IStockTransactionService stockTransactionService)
        {
            _repository = repository;
            _stockTransactionService = stockTransactionService;
        }

        public async Task<bool> CreateAsync(PurchaseOrder order)
        {
            var warehouseId = order.Staff!.Warehouse!.WarehouseId;

            var stockTransactions = new List<StockTransaction>();
            foreach (var detail in order.PurchaseOrderDetails)
            {
                var stockTransaction = new StockTransaction
                {
                    ProductId = detail.ProductId,
                    WarehouseId = warehouseId,
                    Quantity = detail.Quantity,
                    TransactionType = TransactionDirection.IN,
                    TransactionDate = DateTime.UtcNow
                };
                stockTransactions.Add(stockTransaction);
            }
            var stockResult = await _stockTransactionService.AddMultipleStockTransactionsAsync(stockTransactions);
            if (!stockResult)
                return false;

            return await _repository.CreateAsync(order);
        }

        public async Task<PurchaseOrder?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<PurchaseOrder>> GetListAsync()
        {
            return await _repository.GetListAysnc();
        }
    }
}
