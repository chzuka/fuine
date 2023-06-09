namespace Fuine.ViewModels.Pages;

public partial class SubscriptionsViewModel : ObservableObject, INavigationAware
{
    private bool isInitialized = false;

    [ObservableProperty]
    private string links = SubscriptionsService.ReadSubscriptions();

    public void OnNavigatedFrom()
    {

    }

    public void OnNavigatedTo()
    {
        if (!isInitialized)
            InitializeViewModel();
    }

    private void InitializeViewModel()
    {
        isInitialized = true;
    }

    [RelayCommand]
    public static async Task OnUpdateAsync()
    {
        await SubscriptionsService.UpdateSubscriptions();
        await ClashService.ReloadConfigs(Global.Clash配置文件);
    }

    [RelayCommand]
    public static async Task OnRestoreAsync()
    {
        await SubscriptionsService.ReloadSubscriptions();
        await ClashService.ReloadConfigs(Global.Clash配置文件);
    }

    [RelayCommand]
    public async Task OnChangeSubscriptions()
    {
        await SubscriptionsService.SaveSubscriptionsAsync(Links, "clash");
    }

}
