using System;
using System.Collections.Generic;

namespace ERP_API.Entities;

public partial class StockTransaction
{
    public int TransactionId { get; set; }

    public int? ProductId { get; set; }

    public int? WarehouseId { get; set; }

    public int? Quantity { get; set; }

    public string? TransactionType { get; set; } // e.g., "IN", "OUT"


    public DateTime? TransactionDate { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Warehouse? Warehouse { get; set; }
}

public static class TransactionDirection
{
    public const string IN = "IN";
    public const string OUT = "OUT";
}
