using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP_API.tRepositories
{
    public class tCustomerRepository : tICustomerRepository
    {
        private readonly ErpDbContext _dbContext;

        public tCustomerRepository(ErpDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateAsync(Customer entity)
        {
            await _dbContext.Customers.AddAsync(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _dbContext.Customers.ToListAsync();
        }
    }
}