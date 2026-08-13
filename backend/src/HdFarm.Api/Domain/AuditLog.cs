namespace HdFarm.Api.Domain;

public class AuditLog
{
    public long Id { get; set; }
    public long? UserId { get; set; }
    public User? User { get; set; }
    public string Action { get; set; } = default!;
    public string? EntityType { get; set; }
    public long? EntityId { get; set; }
    public string? Detail { get; set; } // JSON dạng chuỗi, map sang jsonb ở DbContext
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
