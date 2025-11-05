public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResultDTO<EmployeeDTO>> GetEmployeesAsync(EmployeeListRequestDTO request)
        {
            if (request.PageNumber < 1)
                request.PageNumber = 1;

            if (request.PageSize < 1 || request.PageSize > 100)
                request.PageSize = 20;

            return await _repository.GetEmployeesAsync(request);
        }

        public async Task<EmployeeDetailDTO> GetEmployeeByIdAsync(int employeeId)
        {
            return await _repository.GetEmployeeByIdAsync(employeeId);
        }

        public async Task<ApiResponse<int>> CreateEmployeeAsync(CreateEmployeeDTO dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.FullName))
                    return new ApiResponse<int>
                    {
                        Success = false,
                        Message = "Full name is required"
                    };

                if (dto.Salary < 0)
                    return new ApiResponse<int>
                    {
                        Success = false,
                        Message = "Salary must be >= 0"
                    };

                var employeeId = await _repository.CreateEmployeeAsync(dto);

                return new ApiResponse<int>
                {
                    Success = true,
                    Message = "Employee created successfully",
                    Data = employeeId
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<int>
                {
                    Success = false,
                    Message = $"Error creating employee: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<bool>> UpdateEmployeeAsync(UpdateEmployeeDTO dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.FullName))
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Full name is required"
                    };

                var result = await _repository.UpdateEmployeeAsync(dto);

                if (!result)
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Employee not found"
                    };

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Employee updated successfully",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error updating employee: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteEmployeeAsync(int employeeId)
        {
            try
            {
                var result = await _repository.DeleteEmployeeAsync(employeeId);

                if (!result)
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Employee not found"
                    };

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = "Employee deleted successfully",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Error deleting employee: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<List<EmployeeReportDTO>>> GetEmployeeReportAsync(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var report = await _repository.GetEmployeeReportAsync(fromDate, toDate);

                return new ApiResponse<List<EmployeeReportDTO>>
                {
                    Success = true,
                    Message = "Employee report retrieved successfully",
                    Data = report
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<EmployeeReportDTO>>
                {
                    Success = false,
                    Message = $"Error retrieving report: {ex.Message}"
                };
            }
        }
    }