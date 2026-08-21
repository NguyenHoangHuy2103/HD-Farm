namespace HdFarm.Api.Domain;

/// <summary>Giá theo cấp khách: lẻ / sỉ / đại lý.</summary>
public class ProductPrice
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public Product Product { get; set; } = default!;
    public PriceTier PriceTier { get; set; } = PriceTier.Retail;
    public decimal Price { get; set; }
    public DateTimeOffset EffectiveFrom { get; set; } = DateTimeOffset.UtcNow;
}
