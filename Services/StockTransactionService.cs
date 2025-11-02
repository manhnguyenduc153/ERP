using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Entities;
using ERP_API.Repositories;

namespace ERP_API.Services
{
    public class StockTransactionService : IStockTransactionService
    {
        private readonly IStockTransactionRepository _repository;

        public StockTransactionService(IStockTransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddMultipleStockTransactionsAsync(IEnumerable<StockTransaction> transactions)
        {
            return await _repository.AddMultipleStockTransactionsAsync(transactions);
        }

        public async Task<bool> AddStockTransactionAsync(StockTransaction transaction)
        {
            return await _repository.AddStockTransactionAsync(transaction);
        }
    }
}