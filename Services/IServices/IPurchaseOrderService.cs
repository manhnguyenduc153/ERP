using ERP_API.Entities;

namespace ERP_API.Services.IServices
{
    public interface IPurchaseOrderService
    {
        Task<List<PurchaseOrder>> GetListAsync();
        Task<PurchaseOrder?> GetByIdAsync(int id);
        Task<bool> CreateAsync(PurchaseOrder order);
    }
}
