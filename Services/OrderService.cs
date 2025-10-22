using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Entities;
using ERP_API.Repositories;
using ERP_API.Services.IServices;

namespace ERP_API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> CreateOrderAsync(SalesOrder order)
        {
            return await _repository.CreateOrderAsync(order);
        }

        public async Task<bool> DeleteOrderAsync(SalesOrder order)
        {
            return await _repository.DeleteOrderAsync(order);
        }

        public async Task<IEnumerable<SalesOrder>> GetAllOrdersAsync()
        {
            return await _repository.GetAllOrdersAsync();
        }

        public async Task<SalesOrder?> GetOrderByIdAsync(int id)
        {
            return await _repository.GetOrderByIdAsync(id);
        }

        public async Task<bool> UpdateOrderAsync(SalesOrder order)
        {
            return await _repository.UpdateOrderAsync(order);
        }
    }
}