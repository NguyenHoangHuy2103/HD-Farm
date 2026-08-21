using NpgsqlTypes;

namespace HdFarm.Api.Domain;

/// <summary>Khớp enum <c>debt_transaction_type</c> trong database/schema.sql.</summary>
public enum DebtTransactionType
{
    [PgName("purchase_on_credit")] PurchaseOnCredit,
    [PgName("repayment")] Repayment
}
