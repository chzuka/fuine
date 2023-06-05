namespace Fuine.Models.Clash;

public sealed class ClashVersion
{
    [JsonPropertyName("meta")]
    public bool? Meta { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;
}
