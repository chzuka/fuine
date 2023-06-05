namespace Fuine.Models.Clash.Proxies;

public sealed class ClashProxyHistory
{
    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("delay")]
    public int Delay { get; set; }
}
