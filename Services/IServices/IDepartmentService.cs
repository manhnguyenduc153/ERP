
namespace ERP_API.Services.IServices
{
    public interface IDepartmentService
    {
        Task<ApiResponse<List<DepartmentDTO>>> GetDepartmentsAsync();
    }
}