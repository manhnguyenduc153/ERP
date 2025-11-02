using ERP_API.Entities;
using ERP_API.Repositories.IRepositories;
using ERP_API.Services.IServices;

namespace ERP_API.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _repository;

        public PurchaseOrderService(IPurchaseOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> CreateAsync(PurchaseOrder order)
        {
            return await _repository.CreateAsync(order);
        }

        public async Task<PurchaseOrder?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<PurchaseOrder>> GetListAsync()
        {
            return await _repository.GetListAysnc();
        }
    }
}
