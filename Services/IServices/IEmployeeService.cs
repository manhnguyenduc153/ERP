public interface IEmployeeService
    {
        Task<PagedResultDTO<EmployeeDTO>> GetEmployeesAsync(EmployeeListRequestDTO request);
        Task<EmployeeDetailDTO> GetEmployeeByIdAsync(int employeeId);
        Task<ApiResponse<int>> CreateEmployeeAsync(CreateEmployeeDTO dto);
        Task<ApiResponse<bool>> UpdateEmployeeAsync(UpdateEmployeeDTO dto);
        Task<ApiResponse<bool>> DeleteEmployeeAsync(int employeeId);
        Task<ApiResponse<List<EmployeeReportDTO>>> GetEmployeeReportAsync(DateTime? fromDate, DateTime? toDate);
    }