namespace Fuine.Services;

public partial class SubscriptionsService
{
    private static readonly IniFileHelper helper = new(Global.Subconverter生成文件);
    public static async Task SaveSubscriptionsAsync(string links)
    {
        string subscriptions = Regex.Replace(links, "[\\r\\n|]{2,}", "|");

        helper.IniWrite("clash", "target", "clash");
        helper.IniWrite("clash", "path", Global.Clash配置文件.Replace("\\", "/"));
        helper.IniWrite("clash", "url", subscriptions);

        var model = Toml.ToModel(await File.ReadAllTextAsync(Global.Subconverter配置文件));
        ((TomlTable)model["common"])["proxy_subscription"] = "SYSTEM";
        await File.WriteAllTextAsync(Global.Subconverter配置文件, Toml.FromModel(model));
    }

    public static string ReadSubscriptions()
    {
        return helper.IniRead("clash", "url").Replace("|", Environment.NewLine);
    }

    public static async Task UpdateSubscriptions()
    {
        var name = Path.Combine(Global.Subconverter目录, "subconverter.exe");

        using Process process = new()
        {
            StartInfo =
            {
                FileName = name,
                Arguments = "-g --artifact \"clash\"",
                CreateNoWindow = true,
            }
        };

        process.Start();
        await process.WaitForExitAsync();
    }
}
