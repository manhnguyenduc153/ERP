namespace ERP_API.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? Description { get; set; }
    }

    public class CategorySearchModel : BaseSearch
    {

    }

    public class CategorySaveModel
    {
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? Description { get; set; }
    }
}
