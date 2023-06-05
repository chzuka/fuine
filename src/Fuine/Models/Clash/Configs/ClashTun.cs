namespace Fuine.Models.Clash.Configs;

public class ClashTun
{
    [JsonPropertyName("enable")]
    public bool Enable { get; set; }

    [JsonPropertyName("device")]
    public string Device { get; set; } = string.Empty;

    [JsonPropertyName("stack")]
    public string Stack { get; set; } = string.Empty;

    [JsonPropertyName("dns-hijack")]
    public string[]? DnsHijack { get; set; }

    [JsonPropertyName("auto-route")]
    public bool AutoRoute { get; set; }

    [JsonPropertyName("auto-detect-interface")]
    public bool AutoDetectInterface { get; set; }
}
