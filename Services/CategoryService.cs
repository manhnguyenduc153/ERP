using ERP_API.Enums;
using ERP_API.Models;
using ERP_API.Entities;
using ERP_API.Models;
using ERP_API.Repositories.IRepositories;
using ERP_API.Services.IServices;
using Mapster;

namespace ERP_API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _CategoryRepository;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(ICategoryRepository CategoryRepository, ILogger<CategoryService> logger)
        {
            _CategoryRepository = CategoryRepository;
            _logger = logger;
        }

        public async Task<ResponseData<IEnumerable<CategoryModel>>> GetListPaging(CategorySearchModel search)
        {
            try
            {
                var totalRecord = await _CategoryRepository.GetTotalRecord(search);
                if (totalRecord > 0)
                {
                    var list = await _CategoryRepository.GetListPaging(search);
                    var pagedList = new PagedList<CategoryModel>(list, totalRecord, search.PageIndex, search.PageSize);
                    return new ResponseData<IEnumerable<CategoryModel>>(true, pagedList, pagedList.GetMetaData());
                }
                return new ResponseData<IEnumerable<CategoryModel>>(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseData<IEnumerable<CategoryModel>>(ex.Message);
            }
        }

        public async Task<ResponseData<object>> Insert(CategorySaveModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.CategoryName))
                    return new ResponseData<object>(ErrorCodeAPI.InvalidInput);

                var entity = model.Adapt<Category>();
                var result = await _CategoryRepository.AddAsync(entity);

                if (result != null)
                    return new ResponseData<object>(true, entity); // trả về entity vừa tạo

                return new ResponseData<object>(ErrorCodeAPI.NotOk);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseData<object>(ex.Message);
            }
        }

        public async Task<ResponseData<object>> Update(CategorySaveModel model)
        {
            try
            {
                if (model.CategoryId <= 0 || string.IsNullOrWhiteSpace(model.CategoryName))
                    return new ResponseData<object>(ErrorCodeAPI.InvalidInput);

                var entity = await _CategoryRepository.GetByIdAsync(model.CategoryId);
                if (entity == null)
                    return new ResponseData<object>(ErrorCodeAPI.NotFound);

                var updateEntity = model.Adapt(entity);
                await _CategoryRepository.UpdateAsync(updateEntity);

                var result = await _CategoryRepository.SaveChangesAsync();

                return new ResponseData<object>(true, updateEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseData<object>(ex.Message);
            }
        }

        public async Task<ResponseData<CategoryModel>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return new ResponseData<CategoryModel>(ErrorCodeAPI.InvalidInput);

                var entity = await _CategoryRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ResponseData<CategoryModel>(ErrorCodeAPI.NotFound);
                }

                var model = entity.Adapt<CategoryModel>();
                return new ResponseData<CategoryModel>(true, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseData<CategoryModel>(ex.Message);
            }
        }

        public async Task<ResponseData<object>> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return new ResponseData<object>(ErrorCodeAPI.InvalidInput);

                var entity = await _CategoryRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ResponseData<object>(ErrorCodeAPI.NotFound);
                }

                await _CategoryRepository.DeleteAsync(entity);
                return new ResponseData<object>(true, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseData<object>(ex.Message);
            }
        }
    }
}
