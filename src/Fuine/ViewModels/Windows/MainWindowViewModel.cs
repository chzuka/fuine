namespace Fuine.ViewModels.Windows;

public partial class MainWindowViewModel : ObservableObject
{
    private bool isInitialized = false;

    [ObservableProperty]
    private string applicationTitle = string.Empty;

    [ObservableProperty]
    private ObservableCollection<object> navigationItems = new();

    [ObservableProperty]
    private ObservableCollection<object> navigationFooter = new();

    [ObservableProperty]
    private Visibility mainWindowVisibility;

    [ObservableProperty]
    private WindowState mainWindowState;

    [ObservableProperty]
    private WindowBackdropType backdropType = (WindowBackdropType)Enum.Parse(typeof(WindowBackdropType), Global.Fuine配置.BackdropType, true);

    public MainWindowViewModel()
    {
        if (!isInitialized)
            InitializeViewModel();
    }

    private void InitializeViewModel()
    {

        if (Global.Fuine配置.TrayStart)
        {
            nint hProcess = Process.GetCurrentProcess().Handle;
            EnergyManager.ToggleEfficiencyMode(hProcess, true);

            MainWindowVisibility = Visibility.Hidden;
            MainWindowState = WindowState.Minimized;
        }

        ApplicationTitle = "Fuine";

        Thickness thickness = new()
        {
            Bottom = 4,
        };

        NavigationItems = new ObservableCollection<object>
        {
            new NavigationViewItem()
            {
                Content = "概览",
                Margin = thickness,
                Icon = new SymbolIcon{ Symbol = SymbolRegular.DataArea24 },
                TargetPageType = typeof(DashboardPage)
            },
            new NavigationViewItem()
            {
                Content = "代理",
                Margin = thickness,
                Icon = new SymbolIcon{ Symbol = SymbolRegular.Globe24 },
                TargetPageType = typeof(ProxiesPage)
            },
            new NavigationViewItem()
            {
                Content = "订阅",
                Margin = thickness,
                Icon = new SymbolIcon{ Symbol = SymbolRegular.Star24 },
                TargetPageType = typeof(SubscriptionsPage)
            },
            new NavigationViewItem()
            {
                Content = "连接",
                Margin = thickness,
                Icon = new SymbolIcon{ Symbol = SymbolRegular.Link24 },
                TargetPageType = typeof(ConnectionsPage)
            },
            new NavigationViewItem()
            {
                Content = "日志",
                Margin = thickness,
                Icon = new SymbolIcon{ Symbol = SymbolRegular.Document24 },
                TargetPageType = typeof(LogsPage)
            }
        };

        NavigationFooter = new ObservableCollection<object> {
            new NavigationViewItem()
            {
                Content = "设置",
                Icon = new SymbolIcon
                {
                    Symbol = SymbolRegular.Settings24
                },
                TargetPageType = typeof(SettingsPage)
            }
        };

        isInitialized = true;
    }

    [RelayCommand]
    private static void OnShow()
    {
        Application.Current.MainWindow.Show();

        nint hProcess = Process.GetCurrentProcess().Handle;
        EnergyManager.ToggleEfficiencyMode(hProcess, false);
    }

    [RelayCommand]
    private static void OnHide(CancelEventArgs e)
    {
        e.Cancel = true;

        Application.Current.MainWindow.Visibility = Visibility.Hidden;

        nint hProcess = Process.GetCurrentProcess().Handle;
        EnergyManager.ToggleEfficiencyMode(hProcess, true);

        Task.Run(() =>
        {
            Thread.Sleep(20000);
            FlushMemoryHelper.FlushMemory();
        });
    }

    [RelayCommand]
    private static void OnLoaded()
    {
        var backdropType = (WindowBackdropType)Enum.Parse(typeof(WindowBackdropType), Global.Fuine配置.BackdropType, true);
        switch (Global.Fuine配置.Theme)
        {
            case "light":
                Theme.Apply(ThemeType.Light, backdropType);
                Watcher.UnWatch();
                break;
            case "dark":
                Theme.Apply(ThemeType.Dark, backdropType);
                Watcher.UnWatch();
                break;
            default:
                Watcher.Watch(Application.Current.MainWindow, backdropType);
                break;
        }
    }
}
