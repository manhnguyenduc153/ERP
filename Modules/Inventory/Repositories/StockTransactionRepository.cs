using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Core.Database.Entities;

namespace ERP_API.Repositories
{
    public class StockTransactionRepository : IStockTransactionRepository
    {
        private readonly ErpDbContext _dbContext;

        public StockTransactionRepository(ErpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddMultipleStockTransactionsAsync(IEnumerable<StockTransaction> transactions)
        {
            await _dbContext.StockTransactions.AddRangeAsync(transactions);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddStockTransactionAsync(StockTransaction transaction)
        {
            await _dbContext.StockTransactions.AddAsync(transaction);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}