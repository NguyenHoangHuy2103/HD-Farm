namespace HdFarm.Api.Domain;

public class OrderReturnItem
{
    public long Id { get; set; }
    public long OrderReturnId { get; set; }
    public OrderReturn OrderReturn { get; set; } = default!;
    public long OrderItemId { get; set; }
    public OrderItem OrderItem { get; set; } = default!;
    public decimal Quantity { get; set; }
}
