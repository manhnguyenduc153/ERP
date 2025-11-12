using ERP_API.Core.Database.Entities;

namespace ERP_API.Repositories.tRepositories
{
    public interface tISupplierRepository
    {
        Task<List<Supplier>> GetListAsync();

        Task<bool> CreateAsync(Supplier supplier);
    }
}
