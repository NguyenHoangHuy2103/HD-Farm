namespace HdFarm.Api.Domain;

public class DeliveryAddress
{
    public long Id { get; set; }
    public long CustomerId { get; set; }
    public Customer Customer { get; set; } = default!;
    public string? RecipientName { get; set; }
    public string? Phone { get; set; }
    public string AddressLine { get; set; } = default!;
    public bool IsDefault { get; set; }
}
