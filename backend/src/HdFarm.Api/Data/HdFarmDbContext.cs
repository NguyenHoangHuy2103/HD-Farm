using HdFarm.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace HdFarm.Api.Data;

public class HdFarmDbContext : DbContext
{
    public HdFarmDbContext(DbContextOptions<HdFarmDbContext> options) : base(options) { }

    // --- Người dùng & phân quyền (5.1) ---
    public DbSet<User> Users => Set<User>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    // --- Sản phẩm & danh mục (5.2) ---
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductUnit> ProductUnits => Set<ProductUnit>();
    public DbSet<ProductPrice> ProductPrices => Set<ProductPrice>();

    // --- Nhà cung cấp (5.7) ---
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();
    public DbSet<SupplierPayment> SupplierPayments => Set<SupplierPayment>();

    // --- Kho & tồn kho (5.3) ---
    public DbSet<StockBatch> StockBatches => Set<StockBatch>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();
    public DbSet<StockTake> StockTakes => Set<StockTake>();
    public DbSet<StockTakeItem> StockTakeItems => Set<StockTakeItem>();

    // --- Khách hàng & công nợ (5.6) ---
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<DeliveryAddress> DeliveryAddresses => Set<DeliveryAddress>();
    public DbSet<DebtTransaction> DebtTransactions => Set<DebtTransaction>();

    // --- Giỏ hàng online (5.5) ---
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    // --- Bán hàng POS (5.4) + online (5.5) ---
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<OrderReturn> OrderReturns => Set<OrderReturn>();
    public DbSet<OrderReturnItem> OrderReturnItems => Set<OrderReturnItem>();

    // --- Hóa đơn điện tử ---
    public DbSet<EInvoice> EInvoices => Set<EInvoice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Các ENUM native của Postgres được khai bằng MapEnum() trong Program.cs
        // (cách chuẩn từ EF Core 9) — không khai lại HasPostgresEnum ở đây.

        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("users");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Username).HasColumnName("username").HasMaxLength(50);
            e.Property(x => x.Phone).HasColumnName("phone").HasMaxLength(20);
            e.Property(x => x.PasswordHash).HasColumnName("password_hash");
            e.Property(x => x.FullName).HasColumnName("full_name").HasMaxLength(150);
            e.Property(x => x.Role).HasColumnName("role");
            e.Property(x => x.IsActive).HasColumnName("is_active");
            e.Property(x => x.FailedLoginAttempts).HasColumnName("failed_login_attempts");
            e.Property(x => x.LockedUntil).HasColumnName("locked_until");
            e.Property(x => x.LastLoginAt).HasColumnName("last_login_at");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            e.Property(x => x.DeletedAt).HasColumnName("deleted_at");
            e.HasIndex(x => x.Username).IsUnique();
            e.HasIndex(x => x.Phone).IsUnique();
        });

        modelBuilder.Entity<AuditLog>(e =>
        {
            e.ToTable("audit_logs");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.Action).HasColumnName("action").HasMaxLength(100);
            e.Property(x => x.EntityType).HasColumnName("entity_type").HasMaxLength(50);
            e.Property(x => x.EntityId).HasColumnName("entity_id");
            e.Property(x => x.Detail).HasColumnName("detail").HasColumnType("jsonb");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
            e.HasIndex(x => x.UserId);
            e.HasIndex(x => new { x.EntityType, x.EntityId });
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        });

        // Nạp toàn bộ IEntityTypeConfiguration trong Data/Configurations/.
        // Thêm nhóm bảng mới = thêm file config, không phải sửa file này.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HdFarmDbContext).Assembly);
    }
}
