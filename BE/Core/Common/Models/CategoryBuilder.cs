using ERP_API.Core.Database.Entities;

public class CategoryBuilder
{
    private int _categoryId;
    private string? _categoryName;
    private string? _description;

    public CategoryBuilder WithId(int id)
    {
        _categoryId = id;
        return this;
    }

    public CategoryBuilder WithName(string? name)
    {
        _categoryName = name;
        return this;
    }

    public CategoryBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }

    public Category Build()
    {
        if (string.IsNullOrWhiteSpace(_categoryName))
            throw new InvalidOperationException("Category name cannot be empty.");

        return new Category
        {
            CategoryId = _categoryId,
            CategoryName = _categoryName,
            Description = _description
        };
    }
}
