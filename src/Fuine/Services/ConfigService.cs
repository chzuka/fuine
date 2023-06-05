namespace Fuine.Services;

internal class ConfigService
{
    public static async Task ReadConfigAsync()
    {
        await TryFindFile(Global.Fuine配置文件);
        Global.Fuine配置 = Toml.ToModel<AppConfig>(await File.ReadAllTextAsync(Global.Fuine配置文件));
    }

    public static async Task WriteConfig()
    {
        await File.WriteAllTextAsync(Global.Fuine配置文件, Toml.FromModel(Global.Fuine配置));
    }

    private static async Task TryFindFile(string name)
    {
        if (!File.Exists(name))
        {
            File.Create(name).Dispose();
            await WriteConfig();
        }
    }
}
