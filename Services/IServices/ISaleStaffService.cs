using ERP_API.Core.Database.Entities;

namespace ERP_API.Services.IServices
{
    public interface ISaleStaffService
    {
        public Task<SaleStaff?> GetSaleStaffByIdAsync(string userId);
    }
}
