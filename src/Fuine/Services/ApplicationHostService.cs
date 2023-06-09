namespace Fuine.Services;

public class ApplicationHostService : IHostedService
{
    private readonly IServiceProvider serviceProvider;
    private INavigationWindow? navigationWindow;

    public ApplicationHostService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
#if !DEBUG
        string mname = Process.GetCurrentProcess().MainModule!.ModuleName;
        string pname = Path.GetFileNameWithoutExtension(mname);
        Process[] processes = Process.GetProcessesByName(pname);

        if (processes.Any(_ => _.Id != Environment.ProcessId))
        {
            if (Environment.GetCommandLineArgs().Contains("-up"))
            {
                await SubscriptionsService.UpdateSubscriptions();
            }
            else
            {
                new ToastContentBuilder()
                    .AddArgument("action", "viewConversation")
                    .AddArgument("conversationId", 9813)
                    .AddText("程序已启动")
                    .Show();
            }

            Application.Current.Shutdown();
        }
        else
#endif
        {
            await ConfigService.ReadConfigAsync();

            _ = Task.Run(async () =>
            {
                RandomPortHelper.GetUsedPort();
                CoreService.StartCore();
                await ClashService.GetConfigAsync();

                if (Global.Fuine配置.AutoUpdate)
                {
                    TaskSchedulerService.AddTask("Fuine", "update");
                }

                if (Global.Fuine配置.SystemProxy)
                {
                    SystemProxyHelper.EnableSystemProxy(Global.Clash配置.Clash本地代理地址);
                }
            }, cancellationToken);

            _ = Task.Run(async () =>
            {
                if (SubscriptionsService.ReadSubscriptions() == string.Empty)
                {
                    return;
                }

                Thread.Sleep(6000);
                await SubscriptionsService.UpdateSubscriptions();
                await ClashService.ReloadConfigs(Global.Clash配置文件);
            }, cancellationToken);

            await HandleActivationAsync();
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (Global.Fuine配置.AutoUpdate)
        {
            TaskSchedulerService.RemoveTask("Fuine", "update");
        }

        if (Global.Fuine配置.SystemProxy)
        {
            SystemProxyHelper.DisableSystemProxy();
        }

        await Task.CompletedTask;
    }

    private async Task HandleActivationAsync()
    {
        await Task.CompletedTask;

        if (!Application.Current.Windows.OfType<MainWindow>().Any())
        {
            navigationWindow = (serviceProvider.
                GetService(typeof(INavigationWindow)) as INavigationWindow)!;
            navigationWindow!.ShowWindow();

            navigationWindow.Navigate(typeof(DashboardPage));
        }

        await Task.CompletedTask;
    }
}
