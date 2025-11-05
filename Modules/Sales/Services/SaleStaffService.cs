using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Entities;
using ERP_API.Repositories.IRepositories;
using ERP_API.Services.IServices;

namespace ERP_API.Services
{
    public class SaleStaffService : ISaleStaffService
    {
        private readonly ISaleStaffRepository _repository;

        public SaleStaffService(ISaleStaffRepository repository)
        {
            _repository = repository;
        }

        public async Task<SaleStaff?> GetSaleStaffByIdAsync(string userId)
        {
            return await _repository.GetSaleStaffByIdAsync(userId);
        }
    }
}