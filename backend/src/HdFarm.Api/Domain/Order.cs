namespace HdFarm.Api.Domain;

/// <summary>
/// Đơn hàng — dùng chung cho POS tại quầy và bán online, phân biệt bằng <see cref="Channel"/>.
/// Đơn POS mặc định <see cref="OrderStatus.Completed"/> vì tạo đơn và thanh toán liền nhau.
/// </summary>
public class Order
{
    public long Id { get; set; }
    public string OrderNo { get; set; } = default!;
    public OrderChannel Channel { get; set; }

    /// <summary>Null với khách vãng lai mua tại quầy không khai báo thông tin.</summary>
    public long? CustomerId { get; set; }
    public Customer? Customer { get; set; }

    /// <summary>Nhân viên lập đơn — null với đơn khách tự đặt online.</summary>
    public long? StaffId { get; set; }
    public User? Staff { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Completed;
    public decimal Subtotal { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public long? DeliveryAddressId { get; set; }
    public DeliveryAddress? DeliveryAddress { get; set; }
    public string? Note { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public EInvoice? EInvoice { get; set; }
}
