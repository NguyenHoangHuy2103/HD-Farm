namespace HdFarm.Api.Domain;

/// <summary>
/// Một lần thanh toán cho đơn. Một đơn có thể có nhiều bản ghi (trả một phần,
/// hoặc kết hợp tiền mặt + chuyển khoản).
/// </summary>
public class Payment
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public Order Order { get; set; } = default!;
    public PaymentMethod Method { get; set; }
    public decimal Amount { get; set; }

    /// <summary>Mã giao dịch từ cổng thanh toán (VNPay/Momo...), null nếu trả tiền mặt.</summary>
    public string? GatewayTxnId { get; set; }

    public DateTimeOffset PaidAt { get; set; } = DateTimeOffset.UtcNow;
}
