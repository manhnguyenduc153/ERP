using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Entities;

namespace ERP_API.Repositories
{
    public interface IStockTransactionRepository
    {
        Task<bool> AddMultipleStockTransactionsAsync(IEnumerable<StockTransaction> transactions);
        Task<bool> AddStockTransactionAsync(StockTransaction transaction);
    }
}