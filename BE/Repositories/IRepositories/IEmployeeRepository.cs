 public interface IEmployeeRepository
    {
        Task<PagedResultDTO<EmployeeDTO>> GetEmployeesAsync(EmployeeListRequestDTO request);
        Task<EmployeeDetailDTO> GetEmployeeByIdAsync(int employeeId);
        Task<int> CreateEmployeeAsync(CreateEmployeeDTO dto);
        Task<bool> UpdateEmployeeAsync(UpdateEmployeeDTO dto);
        Task<bool> DeleteEmployeeAsync(int employeeId);
        Task<List<EmployeeReportDTO>> GetEmployeeReportAsync(DateTime? fromDate, DateTime? toDate);
    }