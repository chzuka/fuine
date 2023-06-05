namespace Fuine.Models.Clash;

public class ClashTraffic
{
    [JsonPropertyName("up")]
    public long Up { get; set; }

    [JsonPropertyName("down")]
    public long Down { get; set; }
}
