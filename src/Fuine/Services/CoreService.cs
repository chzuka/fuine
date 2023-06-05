namespace Fuine.Services;

public class CoreService
{
    private static readonly JobObject job = new();

    public static void StartCore()
    {
        Global.Clash配置.ControllerPort = RandomPortHelper.TryUsePort(9090);
        string name = Path.Combine(Global.Clash目录, "clash.exe");

        string arguments = $"-f {Global.Clash配置文件} -d {Global.Clash目录} -ext-ctl \":{Global.Clash配置.ControllerPort}\"";

        Process process = new()
        {
            StartInfo = new()
            {
                FileName = name,
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false,
            }
        };

        process.Start();

        job.AddProcess(process);
    }

    public static void StopCore(int id)
    {
        Process.GetProcessById(id).Kill();
    }
}
