namespace Fuine.Models.Clash.Proxies;

public class ClashProxyData
{
    [JsonPropertyName("proxies")]
    public Dictionary<string, ClashProxy> Proxies { get; set; } = new();
}
