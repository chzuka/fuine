namespace Fuine.Models.Clash.Proxies;

public sealed class ClashProxyGroup
{
    [JsonPropertyName("all")]
    public List<string> All { get; set; } = new();

    [JsonPropertyName("history")]
    public List<ClashProxyHistory> History { get; set; } = new();

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("now")]
    public string Now { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public ClashProxyType Type { get; set; }

    [JsonPropertyName("udp")]
    public bool Udp { get; set; }

    [JsonPropertyName("xudp")]
    public bool XUdp { get; set; }
}
