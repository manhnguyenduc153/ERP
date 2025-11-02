using System;
using System.Collections.Generic;

namespace ERP_API.Entities;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Position { get; set; }

    public int? DepartmentId { get; set; }

    public DateOnly? HireDate { get; set; }

    public decimal? Salary { get; set; }

    public string? AccountId { get; set; }

    public virtual AspNetUser? Account { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Department? Department { get; set; }

    public virtual ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();

    public virtual PurchaseStaff? PurchaseStaff { get; set; }

    public virtual SaleStaff? SaleStaff { get; set; }
}
