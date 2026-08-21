using HdFarm.Api.Data;
using HdFarm.Api.Domain;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Map các ENUM native của Postgres (đã tạo sẵn trong database/schema.sql).
// Từ EF Core 9 trở lên, MapEnum() đặt trong UseNpgsql() lo trọn cả 2 tầng —
// tầng EF (model + migration) lẫn tầng Npgsql (đọc/ghi lúc chạy).
// KHÔNG dùng modelBuilder.HasPostgresEnum() hay NpgsqlDataSourceBuilder nữa.
// Tham số string là TÊN TYPE trong Postgres (không phải tên schema).
builder.Services.AddDbContext<HdFarmDbContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("Default"),
    o => o
        .MapEnum<UserRole>("user_role")
        .MapEnum<PriceTier>("price_tier")
        .MapEnum<StockMovementType>("stock_movement_type")
        .MapEnum<OrderChannel>("order_channel")
        .MapEnum<OrderStatus>("order_status")
        .MapEnum<PaymentMethod>("payment_method")
        .MapEnum<DebtTransactionType>("debt_transaction_type")
        .MapEnum<EInvoiceStatus>("einvoice_status")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
