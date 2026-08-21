using NpgsqlTypes;

namespace HdFarm.Api.Domain;

/// <summary>Khớp enum <c>einvoice_status</c> trong database/schema.sql.</summary>
public enum EInvoiceStatus
{
    [PgName("pending")] Pending,
    [PgName("issued")] Issued,
    [PgName("error")] Error,
    [PgName("cancelled")] Cancelled
}
