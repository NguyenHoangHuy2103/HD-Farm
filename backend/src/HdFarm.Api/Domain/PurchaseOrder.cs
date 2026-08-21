namespace HdFarm.Api.Domain;

/// <summary>Đơn nhập hàng từ nhà cung cấp.</summary>
public class PurchaseOrder
{
    public long Id { get; set; }
    public long SupplierId { get; set; }
    public Supplier Supplier { get; set; } = default!;

    /// <summary>schema.sql để dạng VARCHAR(30) chứ không phải enum, vd "draft".</summary>
    public string Status { get; set; } = "draft";

    public long? CreatedBy { get; set; }
    public User? CreatedByUser { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
}
