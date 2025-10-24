using ERP_API.Entities;
using ERP_API.Models;

namespace ERP_API.Services.IServices
{
    public interface ISupplierService
    {
        Task<ResponseData<IEnumerable<SupplierModel>>> GetListPaging(SupplierSearchModel search);
        Task<ResponseData<SupplierModel>> GetById(int id);
        Task<ResponseData<object>> Insert(SupplierSaveModel model);
        Task<ResponseData<object>> Update(SupplierSaveModel model);
        Task<ResponseData<object>> Delete(int id);
        Task<bool> CreareSupplierAsync(Supplier supplier);
    }
}
