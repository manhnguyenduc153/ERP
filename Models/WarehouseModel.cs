namespace ERP_API.Models
{
    public class WarehouseModel
    {
        public int WarehouseId { get; set; }

        public string? WarehouseName { get; set; }

        public string? Location { get; set; }
    }

    public class WarehouseSearchModel : BaseSearch
    {

    }

    public class WarehouseSaveModel
    {
        public int WarehouseId { get; set; }

        public string? WarehouseName { get; set; }

        public string? Location { get; set; }
    }
}
