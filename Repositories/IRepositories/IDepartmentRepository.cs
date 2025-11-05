
namespace ERP_API.Repositories.IRepositories
{
    public interface IDepartmentRepository
    {
        Task<List<DepartmentDTO>> GetDepartmentsAsync();
    }
}