using ERP_API.Entities;
using ERP_API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ERP_API.Repositories
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly ErpDbContext _dbContext;

        public PurchaseOrderRepository(ErpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(PurchaseOrder order)
        {
            await _dbContext.PurchaseOrders.AddAsync(order);
            return _dbContext.SaveChanges() > 0;
        }

        public async Task<PurchaseOrder?> GetByIdAsync(int id)
        {
            return await _dbContext.PurchaseOrders
                .Include(o => o.Supplier)
                .Include(o => o.PurchaseOrderDetails)
                .FirstOrDefaultAsync(o => o.PurchaseOrderId == id);
        }

        public async Task<List<PurchaseOrder>> GetListAysnc()
        {
            return await _dbContext.PurchaseOrders
                .Include(o => o.Supplier)
                .Include(o => o.PurchaseOrderDetails)
                .ToListAsync();
        }
    }
}
