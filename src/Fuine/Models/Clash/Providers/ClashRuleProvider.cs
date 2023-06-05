namespace Fuine.Models.Clash.Providers;

public sealed class ClashRuleProvider
{
    public string Behavior { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int RuleCount { get; set; }
    public string Type { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
    public ClashVehicleType VehicleType { get; set; }
}
