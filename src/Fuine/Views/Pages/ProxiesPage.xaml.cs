namespace Fuine.Views.Pages;

public partial class ProxiesPage : INavigableView<ProxiesViewModel>
{
    public ProxiesViewModel ViewModel { get; }
    public ProxiesPage(ProxiesViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }
}
