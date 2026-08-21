namespace HdFarm.Api.Domain;

public class StockTakeItem
{
    public long Id { get; set; }
    public long StockTakeId { get; set; }
    public StockTake StockTake { get; set; } = default!;
    public long ProductId { get; set; }
    public Product Product { get; set; } = default!;
    public long? BatchId { get; set; }
    public StockBatch? Batch { get; set; }
    public decimal SystemQty { get; set; }
    public decimal ActualQty { get; set; }

    /// <summary>
    /// Cột GENERATED ALWAYS AS (actual_qty - system_qty) STORED trong Postgres.
    /// Chỉ đọc — DB tự tính, không gán tay được.
    /// </summary>
    public decimal Diff { get; private set; }
}
