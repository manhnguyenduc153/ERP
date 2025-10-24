using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP_API.Repositories
{
    public class SalesOrderRepository : ISalesOrderRepository
    {
        private readonly ErpDbContext _dbContext;

        public SalesOrderRepository(ErpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateOrderAsync(SalesOrder order)
        {
            await _dbContext.SalesOrders.AddAsync(order);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteOrderAsync(SalesOrder order)
        {
            _dbContext.SalesOrders.Remove(order);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<SalesOrder>> GetAllOrdersAsync()
        {
            return await _dbContext.SalesOrders
                    .Include(o => o.Customer)
                    .Include(o => o.SalesOrderDetails)
                    .ThenInclude(od => od.Product)
                .ToListAsync();
        }

        public async Task<SalesOrder?> GetOrderByIdAsync(int id)
        {
            return await _dbContext.SalesOrders
                .Include(o => o.Customer)
                .Include(o => o.SalesOrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.SalesOrderId == id);
        }

        // Consider update order?
        public async Task<bool> UpdateOrderAsync(SalesOrder order)
        {
            _dbContext.SalesOrders.Update(order);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}