namespace Fuine.Views.Pages;

public partial class SubscriptionsPage : INavigableView<SubscriptionsViewModel>
{
    public SubscriptionsViewModel ViewModel { get; }
    public SubscriptionsPage(SubscriptionsViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }

    #region 拖拽文件
    private readonly IDropTargetHelper helper = (IDropTargetHelper)new DragDropHelper();

    private void DragEnterCommand(object sender, DragEventArgs e)
    {
        e.Effects = DragDropEffects.Link;
        e.Handled = true;
        Point point = PointToScreen(e.GetPosition(this));
        WinPoint wp;
        wp.x = (int)point.X;
        wp.y = (int)point.Y;

        helper.DragEnter(new WindowInteropHelper(
            Application.Current.MainWindow).Handle,
            (IDataObject_Com)e.Data, ref wp, (int)e.Effects);
    }

    private void DragLeaveCommand(object sender, DragEventArgs e)
    {
        helper.DragLeave();
    }

    private void DragOverCommand(object sender, DragEventArgs e)
    {
        e.Effects = DragDropEffects.Link;
        e.Handled = true;
        Point point = PointToScreen(e.GetPosition(this));
        WinPoint wp;
        wp.x = (int)point.X;
        wp.y = (int)point.Y;

        helper.DragOver(ref wp, (int)e.Effects);
    }

    private void DropCommand(object sender, DragEventArgs e)
    {
        e.Effects = DragDropEffects.Link;
        e.Handled = true;
        Point point = PointToScreen(e.GetPosition(this));
        WinPoint wp;
        wp.x = (int)point.X;
        wp.y = (int)point.Y;

        helper.Drop((IDataObject_Com)e.Data, ref wp, (int)e.Effects);

        var files = (Array)e.Data.GetData(DataFormats.FileDrop);

        foreach (string file in files)
        {
            textbox.Text += $"{Environment.NewLine}{file.Replace(@"\", @"/")}";
        }
    }
    #endregion

    private void Textbox_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (!textbox.Text.EndsWith(Environment.NewLine))
        {
            textbox.Text += Environment.NewLine;
            textbox.SelectionStart = textbox.Text.Length;
        }
    }
}
