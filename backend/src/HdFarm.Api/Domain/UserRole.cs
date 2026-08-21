using NpgsqlTypes;

namespace HdFarm.Api.Domain;

/// <summary>Khớp enum <c>user_role</c> trong database/schema.sql.</summary>
public enum UserRole
{
    [PgName("admin")] Admin,
    [PgName("staff")] Staff
}
