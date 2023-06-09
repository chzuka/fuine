namespace Fuine.Services;

public partial class SubscriptionsService
{
    private static readonly IniFileHelper helper = new(Global.Subconverter生成文件);
    public static async Task SaveSubscriptionsAsync(string links, string name)
    {
        string subscriptions = Regex.Replace(links, "[\\r\\n|]{2,}", "|");

        helper.IniWrite(name, "target", "clash");
        helper.IniWrite(name, "path", Global.Clash配置文件.Replace("\\", "/"));
        helper.IniWrite(name, "url", subscriptions);

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
        await Update("clash");
    }

    private static async Task Update(string name)
    {
        using Process process = new()
        {
            StartInfo =
            {
                FileName = Path.Combine(Global.Subconverter目录, "subconverter.exe"),
                Arguments = $"-g --artifact \"{name}\"",
                CreateNoWindow = true,
            }
        };

        process.Start();
        await process.WaitForExitAsync();
    }


    public static async Task ReloadSubscriptions()
    {
        if (!Directory.Exists(Path.Combine(Global.Subconverter目录, "cache")))
        {
            return;
        }

        DirectoryInfo folder = new(Path.Combine(Global.Subconverter目录, "cache"));

        string files = null!;

        foreach (var _ in folder.GetFiles("*"))
        {
            files += $"{Environment.NewLine}{_.FullName.Replace(@"\", @"/")}";
        }

        if (files == null)
        {
            return;
        }

        await SaveSubscriptionsAsync(files, "reload");

        await Update("reload");
    }
}
