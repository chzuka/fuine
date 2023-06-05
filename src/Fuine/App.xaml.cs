namespace Fuine;

public partial class App
{
    private static readonly IHost host = Host
        .CreateDefaultBuilder()
        // 配置应用程序的配置源
        .ConfigureAppConfiguration(_ =>
        {
            // 设置配置文件的基础路径
            _.SetBasePath(AppContext.BaseDirectory);
        })
        // 配置应用程序的服务容器
        .ConfigureServices((context, services) =>
        {
            // 添加应用程序主机服务
            services.AddHostedService<ApplicationHostService>();

            // 添加页面解析服务
            services.AddSingleton<IPageService, PageService>();

            // 添加主题操作服务
            services.AddSingleton<IThemeService, ThemeService>();

            // 添加任务栏操作服务
            services.AddSingleton<ITaskBarService, TaskBarService>();

            // 添加导航服务，与INavigationWindow相同，但没有窗口
            services.AddSingleton<INavigationService, NavigationService>();

            // 添加主窗口和导航窗口
            services.AddScoped<INavigationWindow, MainWindow>();
            services.AddScoped<MainWindowViewModel>();

            // 添加视图和视图模型
            services.AddScoped<DashboardPage>();
            services.AddScoped<DashboardViewModel>();

            services.AddScoped<ProxiesPage>();
            services.AddScoped<ProxiesViewModel>();

            services.AddScoped<SubscriptionsPage>();
            services.AddScoped<SubscriptionsViewModel>();

            services.AddScoped<ConnectionsPage>();
            services.AddScoped<ConnectionsViewModel>();

            services.AddScoped<LogsPage>();
            services.AddScoped<LogsViewModel>();

            services.AddScoped<SettingsPage>();
            services.AddScoped<SettingsViewModel>();

        }).Build();

    /// <summary>
    /// Gets registered service.
    /// </summary>
    /// <typeparam name="T">Type of the service to get.</typeparam>
    /// <returns>Instance of the service or <see langword="null"/>.</returns>
    public static T? GetService<T>()
        where T : class
    {
        return host.Services.GetService(typeof(T)) as T;
    }

    /// <summary>
    /// Occurs when the application is loading.
    /// </summary>
    private async void OnStartup(object sender, StartupEventArgs e)
    {
        await host.StartAsync();
    }

    /// <summary>
    /// Occurs when the application is closing.
    /// </summary>
    private async void OnExit(object sender, ExitEventArgs e)
    {
        await host.StopAsync();

        host.Dispose();
    }

    /// <summary>
    /// Occurs when an exception is thrown by an application but not handled.
    /// </summary>
    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
    }
}
