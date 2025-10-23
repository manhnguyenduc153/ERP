using ERP_API.Entities;
using ERP_API.Enums;
using ERP_API.Models;
using ERP_API.Models;
using ERP_API.Repositories;
using ERP_API.Repositories.IRepositories;
using ERP_API.Services.IServices;
using ERP_API.tRepositories;
using Mapster;

namespace ERP_API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly Repositories.IRepositories.ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerService> _logger;
        private readonly tICustomerRepository _customerRepository2;

        public CustomerService(Repositories.IRepositories.ICustomerRepository CustomerRepository, tICustomerRepository customerRepository2, ILogger<CustomerService> logger)
        {
            _customerRepository = CustomerRepository;
            _customerRepository2 = customerRepository2;
            _logger = logger;
        }

        public async Task<ResponseData<IEnumerable<CustomerModel>>> GetListPaging(CustomerSearchModel search)
        {
            try
            {
                var totalRecord = await _customerRepository.GetTotalRecord(search);
                if (totalRecord > 0)
                {
                    var list = await _customerRepository.GetListPaging(search);
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

                await _customerRepository.AddAsync(entity);

                var result = await _customerRepository.SaveChangesAsync();

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

        public async Task<ResponseData<object>> Update(CustomerSaveModel model)
        {
            try
            {
                if (model.CustomerId <= 0 || string.IsNullOrWhiteSpace(model.Name))
                    return new ResponseData<object>(ErrorCodeAPI.InvalidInput);

                var entity = await _customerRepository.GetByIdAsync(model.CustomerId);
                if (entity == null)
                    return new ResponseData<object>(ErrorCodeAPI.NotFound);

                var updateEntity = model.Adapt(entity);
                await _customerRepository.UpdateAsync(updateEntity);

                var result = await _customerRepository.SaveChangesAsync();

                if (result > 0)
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

        public async Task<ResponseData<CustomerModel>> GetById(int id)
        {
            try
            {
                if (id <= 0)
                    return new ResponseData<CustomerModel>(ErrorCodeAPI.InvalidInput);

                var entity = await _customerRepository.GetByIdAsync(id);
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

                var entity = await _customerRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ResponseData<object>(ErrorCodeAPI.NotFound);
                }

                await _customerRepository.DeleteAsync(entity);

                int result = await _customerRepository.SaveChangesAsync();

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

        public async Task<bool> CreateCustomerAsync(Customer customer)
        {
            return await _customerRepository2.CreateAsync(customer);
        }

        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository2.GetAllAsync();
        }
    }
}
