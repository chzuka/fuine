namespace Fuine.Models.App.Configs;

public class AppConfig
{
    public int MixedPort { get; set; } = 7890;

    public string Mode { get; set; } = ClashModeType.Rule.ToString();

    public string LogLevel { get; set; } = ClashLogLevelType.Silent.ToString();

    public string Theme { get; set; } = "auto";

    public string BackdropType { get; set; } = WindowBackdropType.Tabbed.ToString();

    public bool AutoStart { get; set; }

    public bool TrayStart { get; set; }

    public bool SystemProxy { get; set; }

    public bool Loopback { get; set; }

    public bool AllowLan { get; set; }

    public bool AutoUpdate { get; set; }

    public string DelaySpeedTestUrl { get; set; } = @"https://www.google.com/generate_204";
}
