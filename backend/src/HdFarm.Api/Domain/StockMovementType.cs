using NpgsqlTypes;

namespace HdFarm.Api.Domain;

/// <summary>Khớp enum <c>stock_movement_type</c> trong database/schema.sql.</summary>
public enum StockMovementType
{
    [PgName("import")] Import,
    [PgName("export_sale")] ExportSale,
    [PgName("export_damage")] ExportDamage,
    [PgName("adjustment")] Adjustment
}
