namespace Fuine.Models.Clash.Providers;

public sealed class ClashProxyProvider
{
    public string Name { get; set; } = string.Empty;
    public List<ClashProxyGroup> Proxies { get; set; } = new();
    public string Type { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
    public ClashVehicleType VehicleType { get; set; }
}
