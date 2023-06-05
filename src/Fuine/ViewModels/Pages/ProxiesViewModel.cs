namespace Fuine.ViewModels.Pages;

public partial class ProxiesViewModel : ObservableObject, INavigationAware
{
    [ObservableProperty]
    private ICollection<ProxyGroup> proxyGroupsCollection = Array.Empty<ProxyGroup>();

    [ObservableProperty]
    private ProxyGroup proxyGroupSelectedValue = new();

    [ObservableProperty]
    private Visibility isVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private string proxySelectedValue = string.Empty;

    [ObservableProperty]
    private bool speedTestWait = true;

    [ObservableProperty]
    private SymbolIcon speedTestIcon = new()
    {
        Symbol = SymbolRegular.Flash20,
        Filled = true,
    };

    private bool proxyGroupFirstLoad = true;

    private bool proxyFirstLoad = true;

    public void OnNavigatedFrom()
    {

    }

    public void OnNavigatedTo()
    {
        IsVisibility = Global.Clash配置.Mode == ClashModeType.Global ? Visibility.Collapsed : Visibility.Visible;

        InitializeViewModel();
    }

    private void InitializeViewModel()
    {
        Task.Run(async () =>
        {
            var groups = await ClashService.GetGroupsAsync();

            if (groups.Count == 0)
            {
                return;
            }

            ProxyGroupSelectedValue = groups.FirstOrDefault().Value;

            ProxySelectedValue = ProxyGroupSelectedValue.Now;

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProxyGroupsCollection = groups!.Values;
            });
        });
    }

    [RelayCommand]
    private async Task OnChangeProxyGroupAsync(string parameter)
    {
        if (proxyGroupFirstLoad)
        {
            proxyGroupFirstLoad = false;
        }
        else
        {
            var groups = await ClashService.GetGroupsAsync();
            ProxyGroupsCollection = groups.Values;

            if (parameter != null && groups.ContainsKey(parameter))
            {
                ProxyGroupSelectedValue = groups[parameter];
            }
            else
            {
                ProxyGroupSelectedValue = groups.FirstOrDefault().Value;
            }

            ProxySelectedValue = ProxyGroupSelectedValue.Now;

            _ = Task.Run(ClashService.TestDelay);
        }
    }

    [RelayCommand]
    private async Task OnChangeProxyAsync(object parameters)
    {
        if (proxyFirstLoad)
        {
            proxyFirstLoad = false;
        }
        else
        {
            var proxy = (string)((object[])parameters)[1];

            if (proxy != null)
            {
                var group = (ProxyGroup)((object[])parameters)[0];

                await ClashService.SelectProxy(group.Name, proxy);
            }
        }
    }
}
