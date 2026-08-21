namespace HdFarm.Api.Domain;

/// <summary>
/// Hóa đơn điện tử phát hành qua nhà cung cấp trung gian (Viettel/VNPT/MISA/FPT).
/// Quan hệ 1-1 với <see cref="Order"/> (cột order_id UNIQUE trong schema.sql).
/// </summary>
public class EInvoice
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public Order Order { get; set; } = default!;
    public string? Provider { get; set; }
    public string? InvoiceNo { get; set; }
    public EInvoiceStatus Status { get; set; } = EInvoiceStatus.Pending;
    public DateTimeOffset? IssuedAt { get; set; }
    public string? PdfUrl { get; set; }

    /// <summary>Phản hồi thô từ API nhà cung cấp — JSON dạng chuỗi, map sang jsonb.</summary>
    public string? RawResponse { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
