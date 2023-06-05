namespace Fuine.ViewModels.Pages;

public partial class LogsViewModel : ObservableObject, INavigationAware
{

    private bool isInitialized = false;

    [ObservableProperty]
    private ObservableCollection<Log> logsCollection = new();

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
        Task.Run(async () =>
        {
            await foreach (var _ in ClashService.GetRealTimeLogs())
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (LogsCollection.Count >= 800)
                    {
                        LogsCollection.RemoveAt(LogsCollection.Count - 1);
                    }

                    LogsCollection.Insert(0, _);
                });
            }
        });

        isInitialized = true;
    }
}
