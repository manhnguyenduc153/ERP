using ERP_API.Core.Database.Entities;
using ERP_API.Models;

namespace ERP_API.Repositories.IRepositories
{
    public interface IWarehouseRepository
    {
        Task<IEnumerable<WarehouseModel>> GetListPaging(WarehouseSearchModel model);
        Task<Warehouse> AddAsync(Warehouse entity);
        Task<int> GetTotalRecord(WarehouseSearchModel search);
        Task<Warehouse> GetByIdAsync(int id);
        Task UpdateAsync(Warehouse entity);
        Task DeleteAsync(Warehouse entity);
        Task<int> SaveChangesAsync();
    }
}
