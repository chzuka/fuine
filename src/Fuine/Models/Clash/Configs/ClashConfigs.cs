namespace Fuine.Models.Clash.Configs;

public class ClashConfigs
{
    [JsonPropertyName("port")]
    public int Port { get; set; }

    [JsonPropertyName("socks-port")]
    public int SocksPort { get; set; }

    [JsonPropertyName("redir-port")]
    public int RedirPort { get; set; }

    [JsonPropertyName("tproxy-port")]
    public int TproxyPort { get; set; }

    [JsonPropertyName("mixed-port")]
    public int MixedPort { get; set; }

    [JsonPropertyName("allow-lan")]
    public bool AllowLan { get; set; }

    [JsonPropertyName("bind-address")]
    public string? BindAddress { get; set; }

    [JsonPropertyName("inbound-tfo")]
    public bool InboundTfo { get; set; }

    [JsonPropertyName("mode")]
    public ClashModeType Mode { get; set; }

    [JsonPropertyName("log-level")]
    public ClashLogLevelType LogLevel { get; set; }

    [JsonPropertyName("ipv6")]
    public bool Ipv6 { get; set; }

    [JsonPropertyName("interface-name")]
    public string InterfaceName { get; set; } = string.Empty;

    [JsonPropertyName("geodata-mode")]
    public bool GeodataMode { get; set; }

    [JsonPropertyName("geodata-loader")]
    public string GeodataLoader { get; set; } = string.Empty;

    [JsonPropertyName("tcp-concurrent")]
    public bool TCPConcurrent { get; set; }

    [JsonPropertyName("tun")]
    public ClashTun Tun { get; set; } = new();

    [JsonPropertyName("sniffing")]
    public bool Sniffing { get; set; }
}
