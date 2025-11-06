using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Core.Database.Entities;
using ERP_API.Repositories;
using ERP_API.Services.IServices;

namespace ERP_API.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly ISalesOrderRepository _repository;
        private readonly IStockTransactionService _stockTransactionService;

        public SalesOrderService(ISalesOrderRepository repository, IStockTransactionService stockTransactionService)
        {
            _repository = repository;
            _stockTransactionService = stockTransactionService;
        }

        public async Task<bool> CreateOrderAsync(SalesOrder order)
        {

            var warehouseId = order.Staff!.Store!.WarehouseId;

            var stockTransactions = new List<StockTransaction>();
            foreach (var detail in order.SalesOrderDetails)
            {
                var stockTransaction = new StockTransaction
                {
                    ProductId = detail.ProductId,
                    WarehouseId = warehouseId,
                    Quantity = detail.Quantity,
                    TransactionType = TransactionDirection.OUT,
                    TransactionDate = DateTime.UtcNow
                };
                stockTransactions.Add(stockTransaction);
            }
            var stockResult = await _stockTransactionService.AddMultipleStockTransactionsAsync(stockTransactions);
            if (!stockResult)
                return false;

            return await _repository.CreateOrderAsync(order);
        }

        public async Task<bool> DeleteOrderAsync(SalesOrder order)
        {
            return await _repository.DeleteOrderAsync(order);
        }

        public async Task<IEnumerable<SalesOrder>> GetAllOrdersAsync()
        {
            return await _repository.GetAllOrdersAsync();
        }

        public async Task<SalesOrder?> GetOrderByIdAsync(int id)
        {
            return await _repository.GetOrderByIdAsync(id);
        }

        public async Task<bool> UpdateOrderAsync(SalesOrder order)
        {
            return await _repository.UpdateOrderAsync(order);
        }
    }
}