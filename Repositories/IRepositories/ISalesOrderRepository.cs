using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Core.Database.Entities;

namespace ERP_API.Repositories
{
    public interface ISalesOrderRepository
    {
        Task<IEnumerable<SalesOrder>> GetAllOrdersAsync();
        Task<SalesOrder?> GetOrderByIdAsync(int id);
        Task<bool> CreateOrderAsync(SalesOrder order);
        Task<bool> UpdateOrderAsync(SalesOrder order);
        Task<bool> DeleteOrderAsync(SalesOrder order);
    }
}