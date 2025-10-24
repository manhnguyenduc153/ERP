using ERP_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP_API.Repositories.tRepositories
{
    public class tSupplierRepository : tISupplierRepository
    {
        private readonly ErpDbContext _dbContext;

        public tSupplierRepository(ErpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(Supplier supplier)
        {
            await _dbContext.Suppliers.AddAsync(supplier);
            return _dbContext.SaveChanges() > 0;
        }

        public async Task<List<Supplier>> GetListAysnc()
        {
            return await _dbContext.Suppliers.ToListAsync();
        }
    }
}
