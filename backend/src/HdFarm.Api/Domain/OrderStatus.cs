using NpgsqlTypes;

namespace HdFarm.Api.Domain;

/// <summary>Khớp enum <c>order_status</c> trong database/schema.sql.</summary>
public enum OrderStatus
{
    [PgName("pending")] Pending,
    [PgName("processing")] Processing,
    [PgName("shipping")] Shipping,
    [PgName("completed")] Completed,
    [PgName("cancelled")] Cancelled
}
