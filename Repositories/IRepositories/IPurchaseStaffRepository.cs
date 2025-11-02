using ERP_API.Entities;

namespace ERP_API.Repositories.IRepositories
{
    public interface IPurchaseStaffRepository
    {
        Task<PurchaseStaff?> GetPurchaseStaffByIdAsync(string userId);
    }
}
