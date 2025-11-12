using System;
using System.Collections.Generic;

namespace ERP_API.Core.Database.Entities;

public partial class Attendance
{
    public int AttendanceId { get; set; }

    public int? EmployeeId { get; set; }

    public DateOnly? WorkDate { get; set; }

    public DateTime? CheckIn { get; set; }

    public DateTime? CheckOut { get; set; }

    public string? Status { get; set; }

    public virtual Employee? Employee { get; set; }
}
