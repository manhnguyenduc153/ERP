using ERP_API.Entities;

namespace ERP_API.Repositories.IRepositories
{
    public interface ISaleStaffRepository
    {
        Task<SaleStaff?> GetSaleStaffByIdAsync(string userId);
    }
}
