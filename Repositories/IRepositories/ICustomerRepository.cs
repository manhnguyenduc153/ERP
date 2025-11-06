using ERP_API.Core.Database.Entities;
using ERP_API.Models;

namespace ERP_API.Repositories.IRepositories
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<CustomerModel>> GetListPaging(CustomerSearchModel model);
        Task<Customer> AddAsync(Customer entity);
        Task<int> GetTotalRecord(CustomerSearchModel search);
        Task<Customer> GetByIdAsync(int id);
        Task UpdateAsync(Customer entity);
        Task DeleteAsync(Customer entity);
        Task<int> SaveChangesAsync();
    }
}
