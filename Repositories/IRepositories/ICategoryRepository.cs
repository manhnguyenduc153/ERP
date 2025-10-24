using ERP_API.Entities;
using ERP_API.Models;

namespace ERP_API.Repositories.IRepositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryModel>> GetListPaging(CategorySearchModel model);
        Task<Category> AddAsync(Category entity);
        Task<int> GetTotalRecord(CategorySearchModel search);
        Task<Category> GetByIdAsync(int id);
        Task UpdateAsync(Category entity);
        Task DeleteAsync(Category entity);
        Task<int> SaveChangesAsync();

        Task<List<Category>> GetCategoriesAsync();
    }
}
