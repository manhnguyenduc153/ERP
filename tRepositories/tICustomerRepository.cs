using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Entities;

namespace ERP_API.tRepositories
{
    public interface tICustomerRepository
    {
        Task<bool> CreateAsync(Customer entity);
    }
}