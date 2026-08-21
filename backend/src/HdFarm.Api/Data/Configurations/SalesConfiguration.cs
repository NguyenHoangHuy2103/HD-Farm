using HdFarm.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HdFarm.Api.Data.Configurations;

// Nhóm 4 — Bán hàng POS (5.4) + online (5.5), công nợ (5.6), hóa đơn điện tử.

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> e)
    {
        e.ToTable("orders");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.OrderNo).HasColumnName("order_no").HasMaxLength(30).IsRequired();
        e.Property(x => x.Channel).HasColumnName("channel");
        e.Property(x => x.CustomerId).HasColumnName("customer_id");
        e.Property(x => x.StaffId).HasColumnName("staff_id");
        e.Property(x => x.Status).HasColumnName("status");
        e.Property(x => x.Subtotal).HasColumnName("subtotal").HasPrecision(14, 2);
        e.Property(x => x.DiscountAmount).HasColumnName("discount_amount").HasPrecision(14, 2);
        e.Property(x => x.TotalAmount).HasColumnName("total_amount").HasPrecision(14, 2);
        e.Property(x => x.DeliveryAddressId).HasColumnName("delivery_address_id");
        e.Property(x => x.Note).HasColumnName("note");
        e.Property(x => x.CreatedAt).HasColumnName("created_at");
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at");

        e.HasIndex(x => x.OrderNo).IsUnique();
        e.HasIndex(x => x.CreatedAt);
        e.HasIndex(x => x.CustomerId);
        e.HasIndex(x => new { x.Channel, x.Status });

        e.HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        e.HasOne(x => x.Staff)
            .WithMany()
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Restrict);

        e.HasOne(x => x.DeliveryAddress)
            .WithMany()
            .HasForeignKey(x => x.DeliveryAddressId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> e)
    {
        e.ToTable("order_items");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.OrderId).HasColumnName("order_id");
        e.Property(x => x.ProductId).HasColumnName("product_id");
        e.Property(x => x.BatchId).HasColumnName("batch_id");
        e.Property(x => x.Quantity).HasColumnName("quantity").HasPrecision(14, 3);
        e.Property(x => x.UnitPrice).HasColumnName("unit_price").HasPrecision(14, 2);
        e.Property(x => x.DiscountAmount).HasColumnName("discount_amount").HasPrecision(14, 2);

        // Cột GENERATED ALWAYS ... STORED trong Postgres — EF chỉ đọc, không ghi.
        e.Property(x => x.LineTotal)
            .HasColumnName("line_total")
            .HasPrecision(14, 2)
            .HasComputedColumnSql("quantity * unit_price - discount_amount", stored: true);

        e.HasIndex(x => x.OrderId);
        e.HasIndex(x => x.ProductId);

        e.HasOne(x => x.Order)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        e.HasOne(x => x.Batch)
            .WithMany()
            .HasForeignKey(x => x.BatchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> e)
    {
        e.ToTable("payments");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.OrderId).HasColumnName("order_id");
        e.Property(x => x.Method).HasColumnName("method");
        e.Property(x => x.Amount).HasColumnName("amount").HasPrecision(14, 2);
        e.Property(x => x.GatewayTxnId).HasColumnName("gateway_txn_id").HasMaxLength(100);
        e.Property(x => x.PaidAt).HasColumnName("paid_at");

        e.HasOne(x => x.Order)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class OrderReturnConfiguration : IEntityTypeConfiguration<OrderReturn>
{
    public void Configure(EntityTypeBuilder<OrderReturn> e)
    {
        e.ToTable("order_returns");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.OrderId).HasColumnName("order_id");
        e.Property(x => x.Reason).HasColumnName("reason");
        e.Property(x => x.RefundAmount).HasColumnName("refund_amount").HasPrecision(14, 2);
        e.Property(x => x.ProcessedBy).HasColumnName("processed_by");
        e.Property(x => x.CreatedAt).HasColumnName("created_at");

        e.HasOne(x => x.Order)
            .WithMany()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        e.HasOne(x => x.ProcessedByUser)
            .WithMany()
            .HasForeignKey(x => x.ProcessedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class OrderReturnItemConfiguration : IEntityTypeConfiguration<OrderReturnItem>
{
    public void Configure(EntityTypeBuilder<OrderReturnItem> e)
    {
        e.ToTable("order_return_items");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.OrderReturnId).HasColumnName("order_return_id");
        e.Property(x => x.OrderItemId).HasColumnName("order_item_id");
        e.Property(x => x.Quantity).HasColumnName("quantity").HasPrecision(14, 3);

        e.HasOne(x => x.OrderReturn)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.OrderReturnId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.OrderItem)
            .WithMany()
            .HasForeignKey(x => x.OrderItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class DebtTransactionConfiguration : IEntityTypeConfiguration<DebtTransaction>
{
    public void Configure(EntityTypeBuilder<DebtTransaction> e)
    {
        e.ToTable("debt_transactions");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.CustomerId).HasColumnName("customer_id");
        e.Property(x => x.OrderId).HasColumnName("order_id");
        e.Property(x => x.Type).HasColumnName("type");
        e.Property(x => x.Amount).HasColumnName("amount").HasPrecision(14, 2);
        e.Property(x => x.BalanceAfter).HasColumnName("balance_after").HasPrecision(14, 2);
        e.Property(x => x.CreatedBy).HasColumnName("created_by");
        e.Property(x => x.CreatedAt).HasColumnName("created_at");

        e.HasIndex(x => x.CustomerId);

        e.HasOne(x => x.Customer)
            .WithMany(x => x.DebtTransactions)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        e.HasOne(x => x.Order)
            .WithMany()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        e.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class EInvoiceConfiguration : IEntityTypeConfiguration<EInvoice>
{
    public void Configure(EntityTypeBuilder<EInvoice> e)
    {
        e.ToTable("einvoices");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.OrderId).HasColumnName("order_id");
        e.Property(x => x.Provider).HasColumnName("provider").HasMaxLength(50);
        e.Property(x => x.InvoiceNo).HasColumnName("invoice_no").HasMaxLength(50);
        e.Property(x => x.Status).HasColumnName("status");
        e.Property(x => x.IssuedAt).HasColumnName("issued_at");
        e.Property(x => x.PdfUrl).HasColumnName("pdf_url");
        e.Property(x => x.RawResponse).HasColumnName("raw_response").HasColumnType("jsonb");
        e.Property(x => x.CreatedAt).HasColumnName("created_at");

        e.HasIndex(x => x.OrderId).IsUnique();

        // 1-1 với Order (cột order_id UNIQUE trong schema.sql).
        e.HasOne(x => x.Order)
            .WithOne(x => x.EInvoice)
            .HasForeignKey<EInvoice>(x => x.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
