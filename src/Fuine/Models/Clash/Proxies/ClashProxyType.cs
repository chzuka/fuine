namespace Fuine.Models.Clash.Proxies;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ClashProxyType
{
    Direct,
    Reject,

    Shadowsocks,
    ShadowsocksR,
    Snell,
    Socks5,
    Http,
    Vmess,
    Trojan,

    Relay,
    Selector,
    Fallback,
    URLTest,
    LoadBalance,

    Compatible,
    Pass,

    Vless,
    FallBack
}
