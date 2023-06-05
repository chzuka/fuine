namespace Fuine.Models.Clash.Providers;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ClashVehicleType
{
    Compatible,
    Http,
    File,
    Unknown,
}
