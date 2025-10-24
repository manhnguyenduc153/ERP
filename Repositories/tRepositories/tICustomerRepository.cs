using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Entities;

namespace ERP_API.Repositories.tRepositories
{
    public interface tICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();

        Task<bool> CreateAsync(Customer entity);
    }
}