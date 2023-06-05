namespace Fuine.Helpers;
public class ConvertBytesHelper
{
    #region 字节转换
    public static string ConvertBytes(double? bytes)
    {
        if (bytes is null or <= 0)
            return "0 B";
        string[] sizes = { "B", "KB", "MB", "GB" };
        int order = (int)Math.Log(bytes.Value, 1024);
        double value = bytes.Value / Math.Pow(1024, order);
        return string.Format($"{value:0.##} {sizes[order]}");
    }
    #endregion
}
