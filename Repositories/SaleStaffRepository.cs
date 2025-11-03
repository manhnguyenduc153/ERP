using ERP_API.Entities;
using ERP_API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ERP_API.Repositories
{
    public class SaleStaffRepository : ISaleStaffRepository
    {
        private ErpDbContext _dbContext;

        public SaleStaffRepository(ErpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SaleStaff?> GetSaleStaffByIdAsync(string userId)
        {
            return _dbContext.SaleStaffs
                .Include(ss => ss.Staff)
                .Include(ss => ss.Store)
                .FirstOrDefault(e => e.Staff.AccountId == userId);
        }
    }
}
