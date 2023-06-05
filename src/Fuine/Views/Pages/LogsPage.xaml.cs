using System.Windows.Controls;

namespace Fuine.Views.Pages;

public partial class LogsPage : INavigableView<LogsViewModel>
{
    public LogsViewModel ViewModel { get; }
    public LogsPage(LogsViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;
        InitializeComponent();
    }

    private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
    {
        // 如果没有选中单元格，直接返回
        if (DataGrid.SelectedCells.Count == 0)
        {
            return;
        }

        // 获取选中单元格的值
        string? selectedCellValue = ((Log)DataGrid.SelectedCells[0].Item).PayLoad?.ToString();

        // 将选中单元格的值复制到剪贴板中
        System.Windows.Clipboard.SetDataObject(selectedCellValue);
    }
}
