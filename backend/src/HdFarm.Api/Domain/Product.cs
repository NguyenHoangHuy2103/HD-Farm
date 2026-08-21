namespace HdFarm.Api.Domain;

public class Product
{
    public long Id { get; set; }
    public string Sku { get; set; } = default!;
    public string? Barcode { get; set; }
    public string Name { get; set; } = default!;
    public long? CategoryId { get; set; }
    public Category? Category { get; set; }

    /// <summary>Đơn vị gốc để quy đổi, vd "kg". Các đơn vị khác nằm ở <see cref="Units"/>.</summary>
    public string BaseUnit { get; set; } = default!;

    public string? Manufacturer { get; set; }
    public string? ActiveIngredient { get; set; }
    public string? UsageInstruction { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? DeletedAt { get; set; }

    public ICollection<ProductUnit> Units { get; set; } = new List<ProductUnit>();
    public ICollection<ProductPrice> Prices { get; set; } = new List<ProductPrice>();
}
