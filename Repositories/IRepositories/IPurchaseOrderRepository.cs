using ERP_API.Entities;

namespace ERP_API.Repositories.IRepositories
{
    public interface IPurchaseOrderRepository
    {
        Task<List<PurchaseOrder>> GetListAysnc();
        Task<PurchaseOrder?> GetByIdAsync(int id);
        Task<bool> CreateAsync(PurchaseOrder order);

    }
}
