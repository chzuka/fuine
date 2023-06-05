namespace Fuine.Models.App.Logs;
public class Log
{
    public string Time { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    public string Type { get; set; } = string.Empty;
    public string PayLoad { get; set; } = string.Empty;
}
