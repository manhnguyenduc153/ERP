using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Entities;
using ERP_API.Repositories;

namespace ERP_API.Services.IServices
{
    public interface IOrderService
    {
        Task<IEnumerable<SalesOrder>> GetAllOrdersAsync();
        Task<SalesOrder?> GetOrderByIdAsync(int id);
        Task<bool> CreateOrderAsync(SalesOrder order);
        Task<bool> UpdateOrderAsync(SalesOrder order);
        Task<bool> DeleteOrderAsync(SalesOrder order);
    }

}