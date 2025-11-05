 public class EmployeeListRequestDTO
    {
        public string? SearchTerm { get; set; }
        public int? DepartmentId { get; set; }
        public string? Position { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }