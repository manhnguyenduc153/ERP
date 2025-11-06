using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Core.Database.Entities;
using ERP_API.Repositories;

namespace ERP_API.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;

        public StoreService(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public async Task<Store?> GetStoreByIdAsync(int id)
        {
            return await _storeRepository.GetStoreByIdAsync(id);
        }
    }
}