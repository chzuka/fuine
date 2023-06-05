namespace Fuine.ViewModels.Pages;

public partial class ConnectionsViewModel : ObservableObject, INavigationAware
{
    private bool isInitialized = false;

    [ObservableProperty]
    private ObservableCollectionExtended<Connection> connectionsCollection = new();

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
        SourceList<Connection> connectionsList = new();

        connectionsList.Connect().
           Sort(SortExpressionComparer<Connection>.Descending(_ => _.Host!)).
           ObserveOnDispatcher().
           Bind(ConnectionsCollection).
           Subscribe();

        Task.Run(async () =>
        {
            await foreach (var _ in ClashService.GetRealTimeConnections())
            {
                connectionsList.EditDiff(_.Connections);
            }
        });

        isInitialized = true;
    }
}
