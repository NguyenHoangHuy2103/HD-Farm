namespace HdFarm.Api.Domain;

public class OrderItem
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public Order Order { get; set; } = default!;
    public long ProductId { get; set; }
    public Product Product { get; set; } = default!;

    /// <summary>
    /// Lô hàng xuất ra — gán theo FIFO (lô có exp_date sớm nhất còn tồn) ở tầng
    /// service khi xử lý đơn, không phải trong DB (ghi chú 2 cuối schema.sql).
    /// </summary>
    public long? BatchId { get; set; }
    public StockBatch? Batch { get; set; }

    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Cột GENERATED ALWAYS AS (quantity * unit_price - discount_amount) STORED.
    /// Chỉ đọc — DB tự tính.
    /// </summary>
    public decimal LineTotal { get; private set; }
}
