using System;
using System.Collections.Generic;

namespace ERP_API.Entities;

public partial class Contract
{
    public int ContractId { get; set; }

    public string? ContractType { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public decimal? BaseSalary { get; set; }

    public string? Position { get; set; }

    public string? Status { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }

    public int? EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; }
}
