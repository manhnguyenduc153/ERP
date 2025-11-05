using ERP_API.Repositories;
using ERP_API.Repositories.IRepositories;
using ERP_API.Services.IServices;

namespace ERP_API.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentService(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<List<DepartmentDTO>>> GetDepartmentsAsync()
        {
            try
            {
                var departments = await _repository.GetDepartmentsAsync();

                return new ApiResponse<List<DepartmentDTO>>
                {
                    Success = true,
                    Message = "Departments retrieved successfully",
                    Data = departments
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<DepartmentDTO>>
                {
                    Success = false,
                    Message = $"Error retrieving departments: {ex.Message}"
                };
            }
        }
    }
}