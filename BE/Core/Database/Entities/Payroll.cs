using System;
using System.Collections.Generic;

namespace ERP_API.Core.Database.Entities;

public partial class Payroll
{
    public int PayrollId { get; set; }

    public int? EmployeeId { get; set; }

    public int? Month { get; set; }

    public int? Year { get; set; }

    public decimal? BasicSalary { get; set; }

    public decimal? Bonus { get; set; }

    public decimal? Deductions { get; set; }

    public decimal? NetPay { get; set; }

    public virtual Employee? Employee { get; set; }
}
