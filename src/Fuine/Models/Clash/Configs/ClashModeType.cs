namespace Fuine.Models.Clash.Configs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ClashModeType
{
    Rule,
    Direct,
    Global,
    Script
}
