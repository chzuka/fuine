namespace Fuine.Models.App.Proxies;
public class Proxy
{
    public string Name { get; set; } = null!;
    public ClashProxyType Type { get; set; }
    public int? Delay { get; set; }

    public string? Now { get; set; }
    public List<string>? All { get; set; }
}
