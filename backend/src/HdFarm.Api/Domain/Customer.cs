namespace HdFarm.Api.Domain;

/// <summary>
/// Khách hàng. Dùng chung cho khách mua tại quầy (chỉ cần tên/SĐT) và khách
/// có tài khoản mua online (<see cref="IsOnlineAccount"/> = true, có email + mật khẩu).
/// </summary>
public class Customer
{
    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Phone { get; set; }
    public string? Address { get; set; }

    /// <summary>Diện tích canh tác (ha) — dùng để gợi ý lượng vật tư.</summary>
    public decimal? CultivationArea { get; set; }

    /// <summary>Hạn mức công nợ cho phép. Số dư thực tế tính từ <see cref="DebtTransaction"/>.</summary>
    public decimal CreditLimit { get; set; }

    public bool IsOnlineAccount { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? DeletedAt { get; set; }

    public ICollection<DeliveryAddress> DeliveryAddresses { get; set; } = new List<DeliveryAddress>();
    public ICollection<DebtTransaction> DebtTransactions { get; set; } = new List<DebtTransaction>();
}
