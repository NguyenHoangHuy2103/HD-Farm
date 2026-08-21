using HdFarm.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HdFarm.Api.Data.Configurations;

// Nhóm 2 — Nhà cung cấp (mục 5.7) & Kho/tồn kho (mục 5.3).

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> e)
    {
        e.ToTable("suppliers");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        e.Property(x => x.ContactName).HasColumnName("contact_name").HasMaxLength(150);
        e.Property(x => x.Phone).HasColumnName("phone").HasMaxLength(20);
        e.Property(x => x.Address).HasColumnName("address");
        e.Property(x => x.CreatedAt).HasColumnName("created_at");
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        e.Property(x => x.DeletedAt).HasColumnName("deleted_at");
    }
}

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
{
    public void Configure(EntityTypeBuilder<PurchaseOrder> e)
    {
        e.ToTable("purchase_orders");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.SupplierId).HasColumnName("supplier_id");
        e.Property(x => x.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
        e.Property(x => x.CreatedBy).HasColumnName("created_by");
        e.Property(x => x.CreatedAt).HasColumnName("created_at");

        e.HasOne(x => x.Supplier)
            .WithMany(x => x.PurchaseOrders)
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        e.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class PurchaseOrderItemConfiguration : IEntityTypeConfiguration<PurchaseOrderItem>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderItem> e)
    {
        e.ToTable("purchase_order_items");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.PurchaseOrderId).HasColumnName("purchase_order_id");
        e.Property(x => x.ProductId).HasColumnName("product_id");
        e.Property(x => x.Quantity).HasColumnName("quantity").HasPrecision(14, 3);
        e.Property(x => x.UnitPrice).HasColumnName("unit_price").HasPrecision(14, 2);

        e.HasOne(x => x.PurchaseOrder)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class SupplierPaymentConfiguration : IEntityTypeConfiguration<SupplierPayment>
{
    public void Configure(EntityTypeBuilder<SupplierPayment> e)
    {
        e.ToTable("supplier_payments");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.SupplierId).HasColumnName("supplier_id");
        e.Property(x => x.Amount).HasColumnName("amount").HasPrecision(14, 2);
        e.Property(x => x.PaidAt).HasColumnName("paid_at");
        e.Property(x => x.Note).HasColumnName("note");

        e.HasOne(x => x.Supplier)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class StockBatchConfiguration : IEntityTypeConfiguration<StockBatch>
{
    public void Configure(EntityTypeBuilder<StockBatch> e)
    {
        e.ToTable("stock_batches");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.ProductId).HasColumnName("product_id");
        e.Property(x => x.BatchNo).HasColumnName("batch_no").HasMaxLength(50);
        e.Property(x => x.SupplierId).HasColumnName("supplier_id");
        e.Property(x => x.MfgDate).HasColumnName("mfg_date");
        e.Property(x => x.ExpDate).HasColumnName("exp_date");
        e.Property(x => x.ImportPrice).HasColumnName("import_price").HasPrecision(14, 2);
        e.Property(x => x.QtyReceived).HasColumnName("qty_received").HasPrecision(14, 3);
        e.Property(x => x.QtyRemaining).HasColumnName("qty_remaining").HasPrecision(14, 3);
        e.Property(x => x.CreatedAt).HasColumnName("created_at");

        e.HasIndex(x => x.ProductId);
        e.HasIndex(x => x.ExpDate); // phục vụ xuất kho FIFO + cảnh báo sắp hết hạn

        e.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        e.HasOne(x => x.Supplier)
            .WithMany()
            .HasForeignKey(x => x.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> e)
    {
        e.ToTable("stock_movements");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.BatchId).HasColumnName("batch_id");
        e.Property(x => x.MovementType).HasColumnName("movement_type");
        e.Property(x => x.Quantity).HasColumnName("quantity").HasPrecision(14, 3);
        e.Property(x => x.ReferenceType).HasColumnName("reference_type").HasMaxLength(50);
        e.Property(x => x.ReferenceId).HasColumnName("reference_id");
        e.Property(x => x.CreatedBy).HasColumnName("created_by");
        e.Property(x => x.CreatedAt).HasColumnName("created_at");

        e.HasIndex(x => x.BatchId);

        e.HasOne(x => x.Batch)
            .WithMany(x => x.Movements)
            .HasForeignKey(x => x.BatchId)
            .OnDelete(DeleteBehavior.Restrict);

        e.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class StockTakeConfiguration : IEntityTypeConfiguration<StockTake>
{
    public void Configure(EntityTypeBuilder<StockTake> e)
    {
        e.ToTable("stock_takes");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.TakeDate).HasColumnName("take_date");
        e.Property(x => x.CreatedBy).HasColumnName("created_by");
        e.Property(x => x.Note).HasColumnName("note");
        e.Property(x => x.CreatedAt).HasColumnName("created_at");

        e.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class StockTakeItemConfiguration : IEntityTypeConfiguration<StockTakeItem>
{
    public void Configure(EntityTypeBuilder<StockTakeItem> e)
    {
        e.ToTable("stock_take_items");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.StockTakeId).HasColumnName("stock_take_id");
        e.Property(x => x.ProductId).HasColumnName("product_id");
        e.Property(x => x.BatchId).HasColumnName("batch_id");
        e.Property(x => x.SystemQty).HasColumnName("system_qty").HasPrecision(14, 3);
        e.Property(x => x.ActualQty).HasColumnName("actual_qty").HasPrecision(14, 3);

        // Cột GENERATED ALWAYS ... STORED trong Postgres — EF chỉ đọc, không ghi.
        e.Property(x => x.Diff)
            .HasColumnName("diff")
            .HasPrecision(14, 3)
            .HasComputedColumnSql("actual_qty - system_qty", stored: true);

        e.HasOne(x => x.StockTake)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.StockTakeId)
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
