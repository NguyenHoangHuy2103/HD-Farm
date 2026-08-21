namespace HdFarm.Api.Domain;

/// <summary>Lần thanh toán cho nhà cung cấp.</summary>
public class SupplierPayment
{
    public long Id { get; set; }
    public long SupplierId { get; set; }
    public Supplier Supplier { get; set; } = default!;
    public decimal Amount { get; set; }
    public DateTimeOffset PaidAt { get; set; } = DateTimeOffset.UtcNow;
    public string? Note { get; set; }
}
