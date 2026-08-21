using NpgsqlTypes;

namespace HdFarm.Api.Domain;

/// <summary>Khớp enum <c>payment_method</c> trong database/schema.sql.</summary>
public enum PaymentMethod
{
    [PgName("cash")] Cash,
    [PgName("bank_transfer")] BankTransfer,
    [PgName("debt")] Debt,
    [PgName("gateway")] Gateway,
    [PgName("cod")] Cod
}
