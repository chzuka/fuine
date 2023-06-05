namespace Fuine.Views.Pages;

public partial class ConnectionsPage : INavigableView<ConnectionsViewModel>
{
    public ConnectionsViewModel ViewModel { get; }

    public ConnectionsPage(ConnectionsViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }
}
