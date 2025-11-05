using System;
using System.Collections.Generic;

namespace ERP_API.Entities;

public partial class SaleStaff
{
    public int StaffId { get; set; }

    public int? StoreId { get; set; }

    public virtual ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();

    public virtual Employee Staff { get; set; } = null!;

    public virtual Store? Store { get; set; }
}
