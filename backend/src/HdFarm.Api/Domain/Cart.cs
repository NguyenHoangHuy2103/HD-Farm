namespace HdFarm.Api.Domain;

/// <summary>Giỏ hàng của khách mua online (storefront).</summary>
public class Cart
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public Customer Customer { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}
