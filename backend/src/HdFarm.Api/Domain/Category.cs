namespace HdFarm.Api.Domain;

/// <summary>Danh mục sản phẩm, hỗ trợ cây cha-con qua <see cref="ParentId"/>.</summary>
public class Category
{
    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public long? ParentId { get; set; }
    public Category? Parent { get; set; }
    public ICollection<Category> Children { get; set; } = new List<Category>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? DeletedAt { get; set; }
}
