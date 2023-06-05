namespace Fuine.ViewModels.Pages.Settings;

public partial class SettingsViewModel : ObservableObject, INavigationAware
{
    private bool isInitialized = false;

    [ObservableProperty]
    private string appVersion = $"Fuine - {GetAssemblyVersion()}";

    [ObservableProperty]
    private string delayTestUrl = Global.Fuine配置.DelaySpeedTestUrl;

    [ObservableProperty]
    private bool isAutoStart = Global.Fuine配置.AutoStart;

    [ObservableProperty]
    private bool isTrayStart = Global.Fuine配置.TrayStart;

    [ObservableProperty]
    private bool isAutoUpdate = Global.Fuine配置.AutoUpdate;

    [ObservableProperty]
    private bool isSystemProxy = Global.Fuine配置.SystemProxy;

    [ObservableProperty]
    private bool isLoopback = Global.Fuine配置.Loopback;

    [ObservableProperty]
    private bool allowLan = Global.Clash配置.AllowLan;

    [ObservableProperty]
    private string clashMixedPort = Global.Clash配置.MixedPort.ToString();

    [ObservableProperty]
    private string controllerAddress = Global.Clash配置.Clash外部控制地址.ToString();

    [ObservableProperty]
    private ClashLogLevelType currentLogLevel = Global.Clash配置.LogLevel;

    [ObservableProperty]
    private ClashModeType currentProxyMode = Global.Clash配置.Mode;

    [ObservableProperty]
    private string currentTheme = Global.Fuine配置.Theme;

    [ObservableProperty]
    private string currentBackdropType = Global.Fuine配置.BackdropType;

    public void OnNavigatedTo()
    {
        if (!isInitialized)
            InitializeViewModel();
    }

    public void OnNavigatedFrom()
    {

    }

    private void InitializeViewModel()
    {
        isInitialized = true;
    }

    private static string GetAssemblyVersion()
    {
        return Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;
    }

    [RelayCommand]
    private async Task OnChangeAllowLanAsync()
    {
        await ClashService.PatchConfigAsync("allow-lan", AllowLan);

        Global.Fuine配置.AllowLan = AllowLan;
        await ConfigService.WriteConfig();
    }

    [RelayCommand]
    private async Task OnChangeMixedPortAsync()
    {
        if (string.IsNullOrEmpty(ClashMixedPort))
        {
            ClashMixedPort = Global.Clash配置.MixedPort.ToString();
        }
        else
        {
            await ClashService.PatchConfigAsync("mixed-port",
                Global.Fuine配置.MixedPort =
                Global.Clash配置.MixedPort = Convert.ToInt32(ClashMixedPort));

            if (Global.Fuine配置.SystemProxy)
            {
                SystemProxyHelper.EnableSystemProxy(Global.Clash配置.Clash本地代理地址);
            }

            await ConfigService.WriteConfig();
        }
    }

    [RelayCommand]
    private async Task OnChangeDelayTestUrlAsync()
    {
        Global.Fuine配置.DelaySpeedTestUrl = DelayTestUrl;
        await ConfigService.WriteConfig();
    }

    [RelayCommand]
    private static async Task OnChangeProxyModeAsync(string parameter)
    {
        await ClashService.PatchConfigAsync("mode", parameter);

        Global.Fuine配置.Mode = parameter;

        Global.Clash配置.Mode = (ClashModeType)Enum.Parse(typeof(ClashModeType), parameter, true);
        await ConfigService.WriteConfig();
    }

    [RelayCommand]
    private static async Task OnChangeLogLevelAsync(string parameter)
    {
        await ClashService.PatchConfigAsync("log-level", parameter);

        Global.Fuine配置.LogLevel = parameter;
        await ConfigService.WriteConfig();
    }

    [RelayCommand]
    private async Task OnAutoStartAsync()
    {
        if (IsAutoStart)
        {
            TaskSchedulerService.AddTask("Fuine", "start");
        }
        else
        {
            TaskSchedulerService.RemoveTask("Fuine", "start");
        }

        Global.Fuine配置.AutoStart = IsAutoStart;
        await ConfigService.WriteConfig();
    }

    [RelayCommand]
    private async Task OnTrayStartAsync()
    {
        Global.Fuine配置.TrayStart = IsTrayStart;
        await ConfigService.WriteConfig();
    }

    [RelayCommand]
    private async Task OnAutoUpdateAsync()
    {
        if (IsAutoUpdate)
        {
            TaskSchedulerService.AddTask("Fuine", "update");
        }
        else
        {
            TaskSchedulerService.RemoveTask("Fuine", "update");
        }

        Global.Fuine配置.AutoUpdate = IsAutoUpdate;
        await ConfigService.WriteConfig();
    }

    [RelayCommand]
    private async Task OnSystemProxyAsync()
    {
        if (IsSystemProxy)
        {
            SystemProxyHelper.EnableSystemProxy(Global.Clash配置.Clash本地代理地址);
        }
        else
        {
            SystemProxyHelper.DisableSystemProxy();
        }

        Global.Fuine配置.SystemProxy = IsSystemProxy;
        await ConfigService.WriteConfig();
    }

    [RelayCommand]
    private async Task OnLoopbackAsync()
    {
        if (IsLoopback)
        {
            LoopbackHelper.Filter(true);
        }
        else
        {
            LoopbackHelper.Filter(false);
        }

        Global.Fuine配置.Loopback = IsLoopback;
        await ConfigService.WriteConfig();
    }

    [RelayCommand]
    private static void OnOpenProfiles()
    {
        Process.Start(new ProcessStartInfo()
        {
            FileName = Global.资源目录,
            UseShellExecute = true,
            Verb = "open"
        });
    }

    [RelayCommand]
    private static void OnClearTaskScheduler()
    {
        TaskSchedulerService.RemoveTask("Fuine");
    }

    [RelayCommand]
    private async Task OnChangeThemeAsync(string parameter)
    {
        var backdrop = (WindowBackdropType)Enum.
            Parse(typeof(WindowBackdropType), CurrentBackdropType, true);

        switch (parameter)
        {
            case "light":
                Theme.Apply(ThemeType.Light, backdrop);
                Watcher.UnWatch();
                break;
            case "dark":
                Theme.Apply(ThemeType.Dark, backdrop);
                Watcher.UnWatch();
                break;
            default:
                Watcher.Watch(Application.Current.MainWindow, backdrop);
                break;
        }

        Global.Fuine配置.Theme = CurrentTheme = parameter;
        await ConfigService.WriteConfig();
    }

    [RelayCommand]
    private async Task OnChangeBackdropTypeAsync(string parameter)
    {
        var backdrop = (WindowBackdropType)Enum.
            Parse(typeof(WindowBackdropType), parameter, true);

        if (CurrentTheme != "auto")
        {
            var theme = (ThemeType)Enum.Parse(typeof(ThemeType), CurrentTheme, true);

            Theme.Apply(theme, backdrop);

            Watcher.UnWatch();
        }
        else
        {
            Watcher.Watch(Application.Current.MainWindow, backdrop);
        }

        Global.Fuine配置.BackdropType = CurrentBackdropType = parameter;
        await ConfigService.WriteConfig();
    }
}
