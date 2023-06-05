namespace Fuine.Models.Clash.Connections;

public class ClashMetaData
{
    [JsonPropertyName("network")]
    public string Network { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("sourceIP")]
    public string SourceIP { get; set; } = string.Empty;

    [JsonPropertyName("sourcePort")]
    public string SourcePort { get; set; } = string.Empty;

    [JsonPropertyName("destinationIP")]
    public string DestinationIP { get; set; } = string.Empty;

    [JsonPropertyName("destinationPort")]
    public string DestinationPort { get; set; } = string.Empty;

    [JsonPropertyName("host")]
    public string Host { get; set; } = string.Empty;

    [JsonPropertyName("dnsMode")]
    public string DnsMode { get; set; } = string.Empty;

    [JsonPropertyName("process")]
    public string Process { get; set; } = string.Empty;

    [JsonPropertyName("processPath")]
    public string ProcessPath { get; set; } = string.Empty;
}
