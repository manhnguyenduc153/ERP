using ERP_API.Enums;
using ERP_API.Models;
using ERP_API.Entities;
using ERP_API.Models;
using ERP_API.Repositories.IRepositories;
using ERP_API.Services.IServices;
using Mapster;

namespace ERP_API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _CustomerRepository;
        private readonly ILogger<CustomerService> _logger;
        public CustomerService(ICustomerRepository CustomerRepository, ILogger<CustomerService> logger)
        {
            _CustomerRepository = CustomerRepository;
            _logger = logger;
        }

        public async Task<ResponseData<IEnumerable<CustomerModel>>> GetListPaging(CustomerSearchModel search)
        {
            try
            {
                var totalRecord = await _CustomerRepository.GetTotalRecord(search);
                if (totalRecord > 0)
                {
                    var list = await _CustomerRepository.GetListPaging(search);
                    var pagedList = new PagedList<CustomerModel>(list, totalRecord, search.PageIndex, search.PageSize);
                    return new ResponseData<IEnumerable<CustomerModel>>(true, pagedList, pagedList.GetMetaData());
                }
                return new ResponseData<IEnumerable<CustomerModel>>(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseData<IEnumerable<CustomerModel>>(ex.Message);
            }
        }

        public async Task<ResponseData<object>> Insert(CustomerSaveModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                    return new ResponseData<object>(ErrorCodeAPI.InvalidInput);

                var entity = model.Adapt<Customer>();
                var result = await _CustomerRepository.AddAsync(entity);

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

        public async Task<ResponseData<object>> Update(CustomerSaveModel model)
        {
            try
            {
                if (model.CustomerId <= 0 || string.IsNullOrWhiteSpace(model.Name))
                    return new ResponseData<object>(ErrorCodeAPI.InvalidInput);

                var entity = await _CustomerRepository.GetByIdAsync(model.CustomerId);
                if (entity == null)
                    return new ResponseData<object>(ErrorCodeAPI.NotFound);

                var updateEntity = model.Adapt(entity);
                await _CustomerRepository.UpdateAsync(updateEntity);

                var result = await _CustomerRepository.SaveChangesAsync();

                return new ResponseData<object>(true, updateEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseData<object>(ex.Message);
            }
        }

        public async Task<ResponseData<CustomerModel>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return new ResponseData<CustomerModel>(ErrorCodeAPI.InvalidInput);

                var entity = await _CustomerRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ResponseData<CustomerModel>(ErrorCodeAPI.NotFound);
                }

                var model = entity.Adapt<CustomerModel>();
                return new ResponseData<CustomerModel>(true, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new ResponseData<CustomerModel>(ex.Message);
            }
        }

        public async Task<ResponseData<object>> Delete(int id)
        {
            try
            {
                if (id <= 0)
                    return new ResponseData<object>(ErrorCodeAPI.InvalidInput);

                var entity = await _CustomerRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ResponseData<object>(ErrorCodeAPI.NotFound);
                }

                await _CustomerRepository.DeleteAsync(entity);
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
