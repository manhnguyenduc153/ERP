using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERP_API.Core.Database.Entities;

namespace ERP_API.Services.IServices
{
    public interface IPurchaseStaffService
    {
        Task<PurchaseStaff?> GetPurchaseStaffByIdAsync(string userId);
    }
}