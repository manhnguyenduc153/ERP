namespace ERP_API.Models
{
    public class SupplierModel
    {
        public int SupplierId { get; set; }

        public string? SupplierName { get; set; }

        public string? Contact { get; set; }

        public string? Address { get; set; }
    }

    public class SupplierSearchModel : BaseSearch
    {

    }

    public class SupplierSaveModel
    {
        public int SupplierId { get; set; }

        public string? SupplierName { get; set; }

        public string? Contact { get; set; }

        public string? Address { get; set; }
    }
}
