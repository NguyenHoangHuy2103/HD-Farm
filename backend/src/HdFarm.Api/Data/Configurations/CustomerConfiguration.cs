using HdFarm.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HdFarm.Api.Data.Configurations;

// Nhóm 3 — Khách hàng (mục 5.6) & Giỏ hàng online (mục 5.5).

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> e)
    {
        e.ToTable("customers");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.Name).HasColumnName("name").HasMaxLength(150).IsRequired();
        e.Property(x => x.Phone).HasColumnName("phone").HasMaxLength(20);
        e.Property(x => x.Address).HasColumnName("address");
        e.Property(x => x.CultivationArea).HasColumnName("cultivation_area").HasPrecision(10, 2);
        e.Property(x => x.CreditLimit).HasColumnName("credit_limit").HasPrecision(14, 2);
        e.Property(x => x.IsOnlineAccount).HasColumnName("is_online_account");
        e.Property(x => x.Email).HasColumnName("email").HasMaxLength(150);
        e.Property(x => x.PasswordHash).HasColumnName("password_hash");
        e.Property(x => x.CreatedAt).HasColumnName("created_at");
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        e.Property(x => x.DeletedAt).HasColumnName("deleted_at");

        e.HasIndex(x => x.Phone).IsUnique();
        e.HasIndex(x => x.Email).IsUnique();
    }
}

public class DeliveryAddressConfiguration : IEntityTypeConfiguration<DeliveryAddress>
{
    public void Configure(EntityTypeBuilder<DeliveryAddress> e)
    {
        e.ToTable("delivery_addresses");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.CustomerId).HasColumnName("customer_id");
        e.Property(x => x.RecipientName).HasColumnName("recipient_name").HasMaxLength(150);
        e.Property(x => x.Phone).HasColumnName("phone").HasMaxLength(20);
        e.Property(x => x.AddressLine).HasColumnName("address_line").IsRequired();
        e.Property(x => x.IsDefault).HasColumnName("is_default");

        e.HasOne(x => x.Customer)
            .WithMany(x => x.DeliveryAddresses)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> e)
    {
        e.ToTable("carts");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.CustomerId).HasColumnName("customer_id");
        e.Property(x => x.CreatedAt).HasColumnName("created_at");
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at");

        e.HasOne(x => x.Customer)
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> e)
    {
        e.ToTable("cart_items");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.CartId).HasColumnName("cart_id");
        e.Property(x => x.ProductId).HasColumnName("product_id");
        e.Property(x => x.Quantity).HasColumnName("quantity").HasPrecision(14, 3);

        e.HasIndex(x => new { x.CartId, x.ProductId }).IsUnique();

        e.HasOne(x => x.Cart)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.CartId)
            .OnDelete(DeleteBehavior.Cascade);

        e.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
