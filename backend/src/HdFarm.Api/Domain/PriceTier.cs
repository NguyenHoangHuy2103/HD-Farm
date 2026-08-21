using NpgsqlTypes;

namespace HdFarm.Api.Domain;

/// <summary>Khớp enum <c>price_tier</c> trong database/schema.sql.</summary>
public enum PriceTier
{
    [PgName("retail")] Retail,
    [PgName("wholesale")] Wholesale,
    [PgName("agent")] Agent
}
