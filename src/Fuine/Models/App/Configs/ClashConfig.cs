namespace Fuine.Models.App.Configs;

public class ClashConfig
{
    public int MixedPort { get; set; }

    public bool AllowLan { get; set; }

    public bool IPV6 { get; set; }

    public ClashModeType Mode { get; set; }

    public ClashLogLevelType LogLevel { get; set; }

    public bool TunMode { get; set; }

    public int ControllerPort { get; set; }

    public string Clash本地代理地址
        => $"http://localhost:{MixedPort}";

    public string Clash外部控制地址
        => $"http://localhost:{ControllerPort}";

    public string ClashWebSocket控制地址
        => $"ws://localhost:{ControllerPort}";
}
