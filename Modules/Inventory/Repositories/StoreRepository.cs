using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Entities;

namespace ERP_API.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly ErpDbContext _repository;

        public StoreRepository(ErpDbContext repository)
        {
            _repository = repository;
        }

        public async Task<Store?> GetStoreByIdAsync(int id)
        {
            return await _repository.Stores.FindAsync(id);
        }
    }
}