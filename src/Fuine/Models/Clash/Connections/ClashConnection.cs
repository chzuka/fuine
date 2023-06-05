namespace Fuine.Models.Clash.Connections;

public class ClashConnection
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("metadata")]
    public ClashMetaData MetaData { get; set; } = null!;

    [JsonPropertyName("upload")]
    public long Upload { get; set; }

    [JsonPropertyName("download")]
    public long Download { get; set; }

    [JsonPropertyName("start")]
    public DateTime Start { get; set; }

    [JsonPropertyName("chains")]
    public List<string> Chains { get; set; } = null!;

    [JsonPropertyName("rule")]
    public string Rule { get; set; } = null!;

    [JsonPropertyName("rulePayload")]
    public string RulePayload { get; set; } = null!;
}
