namespace HdFarm.Api.Domain;

/// <summary>Phiếu trả hàng / đổi hàng.</summary>
public class OrderReturn
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public Order Order { get; set; } = default!;
    public string? Reason { get; set; }
    public decimal RefundAmount { get; set; }
    public long? ProcessedBy { get; set; }
    public User? ProcessedByUser { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<OrderReturnItem> Items { get; set; } = new List<OrderReturnItem>();
}
