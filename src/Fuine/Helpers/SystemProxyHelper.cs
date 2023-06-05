using Microsoft.Win32;

namespace Fuine.Helpers;
public class SystemProxyHelper
{
    private static readonly string name = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings";

    public static void EnableSystemProxy(string url)
    {
        Registry.SetValue(name, "ProxyServer", url);
        Registry.SetValue(name, "ProxyEnable", 1);
    }

    public static void DisableSystemProxy()
    {
        Registry.SetValue(name, "ProxyEnable", 0);
    }
}
