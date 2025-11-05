using ERP_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ERP_API.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ErpDbContext _context;

        public EmployeeRepository(ErpDbContext context)
        {
            _context = context;
        }

        private static DateTime ConvertToDateTime(DateOnly? dateOnly)
        {
            return dateOnly.HasValue ? dateOnly.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue;
        }

        private static int CalculateYearsOfService(DateOnly? hireDate)
        {
            if (!hireDate.HasValue)
                return 0;

            var hireDatetime = hireDate.Value.ToDateTime(TimeOnly.MinValue);
            var yearsOfService = DateTime.Now.Year - hireDatetime.Year;

            if (hireDatetime.AddYears(yearsOfService) > DateTime.Now)
                yearsOfService--;

            return yearsOfService;
        }

        private static int CalculateAge(DateOnly? dateOfBirth)
        {
            if (!dateOfBirth.HasValue)
                return 0;

            var dateOfBirthDatetime = dateOfBirth.Value.ToDateTime(TimeOnly.MinValue);
            var age = DateTime.Now.Year - dateOfBirthDatetime.Year;

            if (dateOfBirthDatetime.AddYears(age) > DateTime.Now)
                age--;

            return age;
        }

        public async Task<PagedResultDTO<EmployeeDTO>> GetEmployeesAsync(EmployeeListRequestDTO request)
        {
            var query = _context.Employees
                .Include(e => e.Department)
                .AsQueryable();

            // SEARCH - Thêm Email, Phone
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.ToLower();
                query = query.Where(e =>
                    e.FullName.ToLower().Contains(searchTerm) ||
                    (e.Email != null && e.Email.ToLower().Contains(searchTerm)) ||
                    (e.PhoneNumber != null && e.PhoneNumber.Contains(searchTerm))
                );
            }

            // FILTER
            if (request.DepartmentId.HasValue)
                query = query.Where(e => e.DepartmentId == request.DepartmentId.Value);

            if (!string.IsNullOrEmpty(request.Position))
                query = query.Where(e => e.Position == request.Position);

            var totalRecords = await query.CountAsync();

            var employees = await query
                .OrderByDescending(e => e.HireDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(e => new EmployeeDTO
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FullName,
                    Gender = e.Gender,
                    DateOfBirth = ConvertToDateTime(e.DateOfBirth),
                    Email = e.Email,         
                    Phone = e.PhoneNumber,    
                    Position = e.Position,
                    DepartmentId = e.DepartmentId ?? 0,
                    DepartmentName = e.Department.DepartmentName,
                    HireDate = ConvertToDateTime(e.HireDate),
                    Salary = e.Salary ?? 0m
                })
                .ToListAsync();

            return new PagedResultDTO<EmployeeDTO>
            {
                Data = employees,
                TotalRecords = totalRecords,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public async Task<EmployeeDetailDTO> GetEmployeeByIdAsync(int employeeId)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (employee == null)
                return null;

            return new EmployeeDetailDTO
            {
                EmployeeId = employee.EmployeeId,
                FullName = employee.FullName,
                Gender = employee.Gender,
                DateOfBirth = ConvertToDateTime(employee.DateOfBirth),
                Email = employee.Email,           
                Phone = employee.PhoneNumber,     
                Position = employee.Position,
                DepartmentId = employee.DepartmentId ?? 0,
                DepartmentName = employee.Department?.DepartmentName,
                HireDate = ConvertToDateTime(employee.HireDate),
                Salary = employee.Salary ?? 0m,
                Age = CalculateAge(employee.DateOfBirth),
                YearsOfService = CalculateYearsOfService(employee.HireDate)
            };
        }

        public async Task<int> CreateEmployeeAsync(CreateEmployeeDTO dto)
        {
            var employee = new Employee
            {
                FullName = dto.FullName,
                Gender = dto.Gender,
                DateOfBirth = DateOnly.FromDateTime(dto.DateOfBirth),
                Email = dto.Email,            
                PhoneNumber = dto.Phone,        
                Position = dto.Position,
                DepartmentId = dto.DepartmentId,
                HireDate = DateOnly.FromDateTime(dto.HireDate),
                Salary = dto.Salary
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee.EmployeeId;
        }

        public async Task<bool> UpdateEmployeeAsync(UpdateEmployeeDTO dto)
        {
            var employee = await _context.Employees.FindAsync(dto.EmployeeId);
            if (employee == null)
                return false;

            employee.FullName = dto.FullName;
            employee.Gender = dto.Gender;
            employee.DateOfBirth = DateOnly.FromDateTime(dto.DateOfBirth);
            employee.Email = dto.Email;            
            employee.PhoneNumber = dto.Phone;        
            employee.Position = dto.Position;
            employee.DepartmentId = dto.DepartmentId;
            employee.HireDate = DateOnly.FromDateTime(dto.HireDate);
            employee.Salary = dto.Salary;

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
                return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<EmployeeReportDTO>> GetEmployeeReportAsync(DateTime? fromDate, DateTime? toDate)
        {
            var query = _context.Employees
                .Include(e => e.Department)
                .AsQueryable();

            if (fromDate.HasValue)
            {
                var fromDateOnly = DateOnly.FromDateTime(fromDate.Value);
                query = query.Where(e => e.HireDate >= fromDateOnly);
            }

            if (toDate.HasValue)
            {
                var toDateOnly = DateOnly.FromDateTime(toDate.Value);
                query = query.Where(e => e.HireDate <= toDateOnly);
            }

            var employees = await query
                .OrderByDescending(e => e.HireDate)
                .ToListAsync();

            var report = employees.Select(e =>
            {
                return new EmployeeReportDTO
                {
                    EmployeeId = e.EmployeeId,
                    FullName = e.FullName,
                    Position = e.Position,
                    DepartmentName = e.Department.DepartmentName,
                    HireDate = ConvertToDateTime(e.HireDate),
                    Salary = e.Salary ?? 0m,
                    YearsOfService = CalculateYearsOfService(e.HireDate)
                };
            }).ToList();

            return report;
        }
    }
}