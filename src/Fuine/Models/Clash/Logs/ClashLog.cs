namespace Fuine.Models.Clash.Logs;
public class ClashLog
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("payload")]
    public string PayLoad { get; set; } = string.Empty;
}
