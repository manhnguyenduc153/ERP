using ERP_API.Models;
using ERP_API.Models;

namespace ERP_API.Services.IServices
{
    public interface IWarehouseService
    {
        Task<ResponseData<IEnumerable<WarehouseModel>>> GetListPaging(WarehouseSearchModel search);
        Task<ResponseData<WarehouseModel>> GetById(int id);
        Task<ResponseData<object>> Insert(WarehouseSaveModel model);
        Task<ResponseData<object>> Update(WarehouseSaveModel model);
        Task<ResponseData<object>> Delete(int id);
    }
}
