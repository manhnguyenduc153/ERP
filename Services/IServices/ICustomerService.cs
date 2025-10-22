using ERP_API.Models;
using ERP_API.Models;

namespace ERP_API.Services.IServices
{
    public interface ICustomerService
    {
        Task<ResponseData<IEnumerable<CustomerModel>>> GetListPaging(CustomerSearchModel search);
        Task<ResponseData<CustomerModel>> GetById(int id);
        Task<ResponseData<object>> Insert(CustomerSaveModel model);
        Task<ResponseData<object>> Update(CustomerSaveModel model);
        Task<ResponseData<object>> Delete(int id);
    }
}
