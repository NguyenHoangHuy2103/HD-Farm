namespace HdFarm.Api.Domain;

/// <summary>
/// Sổ công nợ khách hàng. Số dư luôn TÍNH TỪ bảng này, không lưu số dư cố định
/// trên <see cref="Customer"/> (ghi chú 1 cuối schema.sql).
/// </summary>
public class DebtTransaction
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public Customer Customer { get; set; } = default!;

    /// <summary>Null khi là lần trả nợ không gắn với đơn cụ thể.</summary>
    public long? OrderId { get; set; }
    public Order? Order { get; set; }

    public DebtTransactionType Type { get; set; }
    public decimal Amount { get; set; }

    /// <summary>Số dư nợ sau giao dịch này — lưu để đối chiếu, không phải nguồn tính toán.</summary>
    public decimal BalanceAfter { get; set; }

    public long? CreatedBy { get; set; }
    public User? CreatedByUser { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
