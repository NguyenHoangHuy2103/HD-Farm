namespace HdFarm.Api.Domain;

/// <summary>
/// Nhật ký mọi biến động kho (nhập / xuất bán / hủy / điều chỉnh).
/// Tồn kho luôn TÍNH TỪ bảng này, không lưu số dư cố định (ghi chú 1 cuối schema.sql).
/// </summary>
public class StockMovement
{
    public long Id { get; set; }
    public long BatchId { get; set; }
    public StockBatch Batch { get; set; } = default!;
    public StockMovementType MovementType { get; set; }

    /// <summary>Âm khi xuất, dương khi nhập — schema.sql không ràng buộc dấu.</summary>
    public decimal Quantity { get; set; }

    /// <summary>Trỏ mềm tới chứng từ nguồn, vd "order" / "stock_take". Không có FK trong DB.</summary>
    public string? ReferenceType { get; set; }
    public long? ReferenceId { get; set; }

    public long? CreatedBy { get; set; }
    public User? CreatedByUser { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
