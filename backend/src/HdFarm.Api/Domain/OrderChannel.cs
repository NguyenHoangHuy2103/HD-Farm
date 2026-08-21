using NpgsqlTypes;

namespace HdFarm.Api.Domain;

/// <summary>Khớp enum <c>order_channel</c> trong database/schema.sql.</summary>
public enum OrderChannel
{
    [PgName("pos")] Pos,
    [PgName("online")] Online
}
