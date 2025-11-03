using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Entities;

namespace ERP_API.Services
{
    public interface IStockTransactionService
    {
        Task<bool> AddStockTransactionAsync(StockTransaction transaction);

        Task<bool> AddMultipleStockTransactionsAsync(IEnumerable<StockTransaction> transactions);
    }
}