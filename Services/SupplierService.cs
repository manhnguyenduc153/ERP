using ERP_API.Entities;
using ERP_API.Enums;
using ERP_API.Models;
using ERP_API.Models;
using ERP_API.Repositories;
using ERP_API.Repositories.IRepositories;
using ERP_API.Services.IServices;
using Mapster;

namespace ERP_API.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ILogger<SupplierService> _logger;
        public SupplierService(ISupplierRepository SupplierRepository, ILogger<SupplierService> logger)
        {
            _supplierRepository = SupplierRepository;
            _logger = logger;
        }

        public async Task<ResponseData<IEnumerable<SupplierModel>>> GetListPaging(SupplierSearchModel search)
        {
            try
            {
                var totalRecord = await _supplierRepository.GetTotalRecord(search);
                if (totalRecord > 0)
                {
                    var list = await _supplierRepository.GetListPaging(search);
                    var pagedList = new PagedList<SupplierModel>(list, totalRecord, search.PageIndex, search.PageSize);
                    return new ResponseData<IEnumerable<SupplierModel>>(true, pagedList, pagedList.GetMetaData());
                }
                return new ResponseData<IEnumerable<SupplierModel>>(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseData<IEnumerable<SupplierModel>>(ex.Message);
            }
        }

        public async Task<ResponseData<object>> Insert(SupplierSaveModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.SupplierName))
                    return new ResponseData<object>(ErrorCodeAPI.InvalidInput);

                var entity = model.Adapt<Supplier>();

                await _supplierRepository.AddAsync(entity);

                var result = await _supplierRepository.SaveChangesAsync();

                if (result > 0)
                {
                    return new ResponseData<object>(true, entity);
                }

                return new ResponseData<object>(ErrorCodeAPI.NotOk);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseData<object>(ex.Message);
            }
        }

        public async Task<ResponseData<object>> Update(SupplierSaveModel model)
        {
            try
            {
                if (model.SupplierId <= 0 || string.IsNullOrWhiteSpace(model.SupplierName))
                    return new ResponseData<object>(ErrorCodeAPI.InvalidInput);

                var entity = await _supplierRepository.GetByIdAsync(model.SupplierId);
                if (entity == null)
                    return new ResponseData<object>(ErrorCodeAPI.NotFound);

                var updateEntity = model.Adapt(entity);
                await _supplierRepository.UpdateAsync(updateEntity);

                var result = await _supplierRepository.SaveChangesAsync();

                if(result > 0)
                {
                    return new ResponseData<object>(true, updateEntity);
                }

                return new ResponseData<object>(ErrorCodeAPI.NotOk);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseData<object>(ex.Message);
            }
        }

        public async Task<ResponseData<SupplierModel>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return new ResponseData<SupplierModel>(ErrorCodeAPI.InvalidInput);

                var entity = await _supplierRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ResponseData<SupplierModel>(ErrorCodeAPI.NotFound);
                }

                var model = entity.Adapt<SupplierModel>();
                return new ResponseData<SupplierModel>(true, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseData<SupplierModel>(ex.Message);
            }
        }

        public async Task<ResponseData<object>> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return new ResponseData<object>(ErrorCodeAPI.InvalidInput);

                var entity = await _supplierRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ResponseData<object>(ErrorCodeAPI.NotFound);
                }

                await _supplierRepository.DeleteAsync(entity);

                int result = await _supplierRepository.SaveChangesAsync();

                if (result > 0)
                    return new ResponseData<object>(true, entity);

                return new ResponseData<object>(ErrorCodeAPI.NotOk);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseData<object>(ex.Message);
            }
        }
    }
}
