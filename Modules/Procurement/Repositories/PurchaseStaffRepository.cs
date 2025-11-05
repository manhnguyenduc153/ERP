using ERP_API.Entities;
using ERP_API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ERP_API.Repositories
{
    public class PurchaseStaffRepository : IPurchaseStaffRepository
    {
        private readonly ErpDbContext _dbContext;

        public PurchaseStaffRepository(ErpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PurchaseStaff?> GetPurchaseStaffByIdAsync(string userId)
        {
            return await _dbContext.PurchaseStaffs
                .Include(s => s.Staff)
                .Include(s => s.Warehouse)
                .FirstOrDefaultAsync(s => s.Staff.AccountId == userId);
        }

    }
}
