namespace Fuine.Models.Clash.Configs;

public class ClashUpdateConfigRequest
{
    [JsonPropertyName("path")]
    public string Path { get; set; } = string.Empty;
    [JsonPropertyName("payload")]
    public string Payload { get; set; } = string.Empty;
}
