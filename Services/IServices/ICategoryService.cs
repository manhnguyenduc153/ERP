using ERP_API.Entities;
using ERP_API.Models;
using ERP_API.Models;

namespace ERP_API.Services.IServices
{
    public interface ICategoryService
    {
        Task<ResponseData<IEnumerable<CategoryModel>>> GetListPaging(CategorySearchModel search);
        Task<ResponseData<CategoryModel>> GetById(int id);
        Task<ResponseData<object>> Insert(CategorySaveModel model);
        Task<ResponseData<object>> Update(CategorySaveModel model);
        Task<ResponseData<object>> Delete(int id);
        Task<List<Category>> GetCategoriesAsync();
    }
}
