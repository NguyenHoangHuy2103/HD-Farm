using HdFarm.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HdFarm.Api.Data.Configurations;

// Nhóm 1 — Sản phẩm & danh mục (mục 5.2 tài liệu yêu cầu).

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> e)
    {
        e.ToTable("categories");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        e.Property(x => x.ParentId).HasColumnName("parent_id");
        e.Property(x => x.CreatedAt).HasColumnName("created_at");
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        e.Property(x => x.DeletedAt).HasColumnName("deleted_at");

        e.HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> e)
    {
        e.ToTable("products");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.Sku).HasColumnName("sku").HasMaxLength(50).IsRequired();
        e.Property(x => x.Barcode).HasColumnName("barcode").HasMaxLength(50);
        e.Property(x => x.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
        e.Property(x => x.CategoryId).HasColumnName("category_id");
        e.Property(x => x.BaseUnit).HasColumnName("base_unit").HasMaxLength(20).IsRequired();
        e.Property(x => x.Manufacturer).HasColumnName("manufacturer").HasMaxLength(150);
        e.Property(x => x.ActiveIngredient).HasColumnName("active_ingredient");
        e.Property(x => x.UsageInstruction).HasColumnName("usage_instruction");
        e.Property(x => x.Description).HasColumnName("description");
        e.Property(x => x.IsActive).HasColumnName("is_active");
        e.Property(x => x.CreatedAt).HasColumnName("created_at");
        e.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        e.Property(x => x.DeletedAt).HasColumnName("deleted_at");

        e.HasIndex(x => x.Sku).IsUnique();
        e.HasIndex(x => x.Barcode).IsUnique();
        e.HasIndex(x => x.CategoryId);

        e.HasOne(x => x.Category)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ProductUnitConfiguration : IEntityTypeConfiguration<ProductUnit>
{
    public void Configure(EntityTypeBuilder<ProductUnit> e)
    {
        e.ToTable("product_units");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.ProductId).HasColumnName("product_id");
        e.Property(x => x.UnitName).HasColumnName("unit_name").HasMaxLength(20).IsRequired();
        e.Property(x => x.ConversionRate).HasColumnName("conversion_rate").HasPrecision(12, 3);
        e.Property(x => x.IsDefault).HasColumnName("is_default");

        e.HasIndex(x => new { x.ProductId, x.UnitName }).IsUnique();

        e.HasOne(x => x.Product)
            .WithMany(x => x.Units)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ProductPriceConfiguration : IEntityTypeConfiguration<ProductPrice>
{
    public void Configure(EntityTypeBuilder<ProductPrice> e)
    {
        e.ToTable("product_prices");
        e.HasKey(x => x.Id);
        e.Property(x => x.Id).HasColumnName("id");
        e.Property(x => x.ProductId).HasColumnName("product_id");
        e.Property(x => x.PriceTier).HasColumnName("price_tier");
        e.Property(x => x.Price).HasColumnName("price").HasPrecision(14, 2);
        e.Property(x => x.EffectiveFrom).HasColumnName("effective_from");

        e.HasIndex(x => new { x.ProductId, x.PriceTier }).IsUnique();

        e.HasOne(x => x.Product)
            .WithMany(x => x.Prices)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
