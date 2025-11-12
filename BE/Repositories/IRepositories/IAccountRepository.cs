using ERP_API.Models;

namespace ERP_API.Repositories.IRepositories
{
    public interface IAccountRepository
    {
        Task<long> GetTotalRecord(AccountSearchModel search);
        Task<IEnumerable<AccountModel>> GetListPaging(AccountSearchModel search);
    }
}
