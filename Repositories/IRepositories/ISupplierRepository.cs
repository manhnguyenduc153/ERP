using ERP_API.Core.Database.Entities;
using ERP_API.Models;

namespace ERP_API.Repositories.IRepositories
{
    public interface ISupplierRepository
    {
        Task<IEnumerable<SupplierModel>> GetListPaging(SupplierSearchModel model);
        Task<Supplier> AddAsync(Supplier entity);
        Task<int> GetTotalRecord(SupplierSearchModel search);
        Task<Supplier> GetByIdAsync(int id);
        Task UpdateAsync(Supplier entity);
        Task DeleteAsync(Supplier entity);
        Task<int> SaveChangesAsync();
    }
}
