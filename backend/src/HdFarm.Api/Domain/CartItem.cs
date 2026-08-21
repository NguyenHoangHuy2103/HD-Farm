namespace HdFarm.Api.Domain;

public class CartItem
{
    public long Id { get; set; }
    public long CartId { get; set; }
    public Cart Cart { get; set; } = default!;
    public long ProductId { get; set; }
    public Product Product { get; set; } = default!;
    public decimal Quantity { get; set; }
}
