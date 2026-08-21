namespace HdFarm.Api.Domain;

/// <summary>Phiếu kiểm kho định kỳ.</summary>
public class StockTake
{
    public long Id { get; set; }
    public DateOnly TakeDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public long? CreatedBy { get; set; }
    public User? CreatedByUser { get; set; }
    public string? Note { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<StockTakeItem> Items { get; set; } = new List<StockTakeItem>();
}
