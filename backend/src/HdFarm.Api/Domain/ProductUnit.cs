namespace HdFarm.Api.Domain;

/// <summary>Quy đổi đơn vị, vd 1 bao = 50 kg (<see cref="ConversionRate"/> = 50).</summary>
public class ProductUnit
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public Product Product { get; set; } = default!;
    public string UnitName { get; set; } = default!;
    public decimal ConversionRate { get; set; }
    public bool IsDefault { get; set; }
}
