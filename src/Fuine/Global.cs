namespace Fuine;

internal class Global
{
    public static readonly string 应用目录 =
       Path.GetDirectoryName(Environment.ProcessPath!)!;

    public static readonly string 资源目录 =
        Path.Combine(应用目录, "resources");

    public static readonly string Clash目录 =
        Path.Combine(资源目录, "clash");
    public static readonly string Clash配置文件 =
        Path.Combine(Clash目录, "config.yaml");

    public static readonly string Subconverter目录 =
        Path.Combine(资源目录, "subconverter");
    public static readonly string Subconverter生成文件 =
        Path.Combine(Subconverter目录, "generate.ini");
    public static readonly string Subconverter配置文件 =
        Path.Combine(Subconverter目录, "pref.toml");

    public static readonly string 订阅文件 =
        Path.Combine(资源目录, "sub.txt");
    public static readonly string Fuine配置文件 =
        Path.Combine(资源目录, "config.toml");

    public static AppConfig Fuine配置 { get; set; } = new();
    public static ClashConfig Clash配置 { get; set; } = new();
}
