using System;
using System.Collections.Generic;

namespace ERP_API.Core.Database.Entities;

public partial class Store
{
    public int StoreId { get; set; }

    public string StoreName { get; set; } = null!;

    public string? Location { get; set; }

    public int? WarehouseId { get; set; }

    public virtual ICollection<SaleStaff> SaleStaffs { get; set; } = new List<SaleStaff>();

    public virtual Warehouse? Warehouse { get; set; }
}
