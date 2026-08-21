namespace HdFarm.Api.Domain;

/// <summary>
/// Lô hàng — cốt lõi để theo dõi hạn dùng và xuất kho theo FIFO.
/// <see cref="QtyRemaining"/> chỉ được đổi kèm 1 bản ghi <see cref="StockMovement"/> tương ứng.
/// </summary>
public class StockBatch
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public Product Product { get; set; } = default!;
    public string? BatchNo { get; set; }
    public long? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public DateOnly? MfgDate { get; set; }
    public DateOnly? ExpDate { get; set; }
    public decimal ImportPrice { get; set; }
    public decimal QtyReceived { get; set; }
    public decimal QtyRemaining { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<StockMovement> Movements { get; set; } = new List<StockMovement>();
}
