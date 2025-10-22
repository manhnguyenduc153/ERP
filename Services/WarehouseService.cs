using ERP_API.Enums;
using ERP_API.Models;
using ERP_API.Entities;
using ERP_API.Models;
using ERP_API.Repositories.IRepositories;
using ERP_API.Services.IServices;
using Mapster;

namespace ERP_API.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository _WarehouseRepository;
        private readonly ILogger<WarehouseService> _logger;
        public WarehouseService(IWarehouseRepository WarehouseRepository, ILogger<WarehouseService> logger)
        {
            _WarehouseRepository = WarehouseRepository;
            _logger = logger;
        }

        public async Task<ResponseData<IEnumerable<WarehouseModel>>> GetListPaging(WarehouseSearchModel search)
        {
            try
            {
                var totalRecord = await _WarehouseRepository.GetTotalRecord(search);
                if (totalRecord > 0)
                {
                    var list = await _WarehouseRepository.GetListPaging(search);
                    var pagedList = new PagedList<WarehouseModel>(list, totalRecord, search.PageIndex, search.PageSize);
                    return new ResponseData<IEnumerable<WarehouseModel>>(true, pagedList, pagedList.GetMetaData());
                }
                return new ResponseData<IEnumerable<WarehouseModel>>(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseData<IEnumerable<WarehouseModel>>(ex.Message);
            }
        }

        public async Task<ResponseData<object>> Insert(WarehouseSaveModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.WarehouseName))
                    return new ResponseData<object>(ErrorCodeAPI.InvalidInput);

                var entity = model.Adapt<Warehouse>();
                var result = await _WarehouseRepository.AddAsync(entity);

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

        public async Task<ResponseData<object>> Update(WarehouseSaveModel model)
        {
            try
            {
                if (model.WarehouseId <= 0 || string.IsNullOrWhiteSpace(model.WarehouseName))
                    return new ResponseData<object>(ErrorCodeAPI.InvalidInput);

                var entity = await _WarehouseRepository.GetByIdAsync(model.WarehouseId);
                if (entity == null)
                    return new ResponseData<object>(ErrorCodeAPI.NotFound);

                var updateEntity = model.Adapt(entity);
                await _WarehouseRepository.UpdateAsync(updateEntity);

                var result = await _WarehouseRepository.SaveChangesAsync();

                return new ResponseData<object>(true, updateEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseData<object>(ex.Message);
            }
        }

        public async Task<ResponseData<WarehouseModel>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return new ResponseData<WarehouseModel>(ErrorCodeAPI.InvalidInput);

                var entity = await _WarehouseRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ResponseData<WarehouseModel>(ErrorCodeAPI.NotFound);
                }

                var model = entity.Adapt<WarehouseModel>();
                return new ResponseData<WarehouseModel>(true, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseData<WarehouseModel>(ex.Message);
            }
        }

        public async Task<ResponseData<object>> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return new ResponseData<object>(ErrorCodeAPI.InvalidInput);

                var entity = await _WarehouseRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ResponseData<object>(ErrorCodeAPI.NotFound);
                }

                await _WarehouseRepository.DeleteAsync(entity);
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
