using ERP_API.Core.Database.Entities;
using ERP_API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ERP_API.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ErpDbContext _context;

        public DepartmentRepository(ErpDbContext context)
        {
            _context = context;
        }

        public async Task<List<DepartmentDTO>> GetDepartmentsAsync()
        {
            var departments = await _context.Departments
                .OrderBy(d => d.DepartmentName)
                .Select(d => new DepartmentDTO
                {
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.DepartmentName,
                    Description = d.Description
                })
                .ToListAsync();

            return departments;
        }
    }
}