namespace ERP_API.Models
{
    public class CustomerModel
    {
        public int CustomerId { get; set; }

        public string? Name { get; set; }

        public string? Contact { get; set; }

        public string? Address { get; set; }
    }

    public class CustomerSearchModel : BaseSearch
    {
        
    }

    public class CustomerSaveModel
    {
        public int CustomerId { get; set; }

        public string? Name { get; set; }

        public string? Contact { get; set; }

        public string? Address { get; set; }
    }
}
