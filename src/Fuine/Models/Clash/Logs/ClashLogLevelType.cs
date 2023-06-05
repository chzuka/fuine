namespace Fuine.Models.Clash.Logs;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ClashLogLevelType
{
    Silent,
    Info,
    Debug,
    Error,
    Warning
}
