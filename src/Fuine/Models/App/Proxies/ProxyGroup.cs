namespace Fuine.Models.App.Proxies;
public class ProxyGroup
{
    public string Name { get; set; } = null!;

    public string Now { get; set; } = string.Empty;

    public int? Delay { get; set; }

    public ClashProxyType Type { get; set; }

    public List<Proxy> Proxies { get; set; } = new();
}
