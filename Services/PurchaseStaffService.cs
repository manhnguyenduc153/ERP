using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Entities;
using ERP_API.Repositories.IRepositories;
using ERP_API.Services.IServices;

namespace ERP_API.Services
{
    public class PurchaseStaffService : IPurchaseStaffService
    {
        private readonly IPurchaseStaffRepository _repository;

        public PurchaseStaffService(IPurchaseStaffRepository repository)
        {
            _repository = repository;
        }

        public async Task<PurchaseStaff?> GetPurchaseStaffByIdAsync(string userId)
        {
            return await _repository.GetPurchaseStaffByIdAsync(userId);
        }
    }
}