namespace Fuine.ViewModels.Pages;
public partial class DashboardViewModel : ObservableObject, INavigationAware
{
    private bool isInitialized;

    public Axis[] XAxes { get; set; } = { new() { Labeler = _ => string.Empty, LabelsPaint = null } };

    public Axis[] YAxes { get; set; } = {
        new() { MinLimit = 0, TextSize = 12,
            LabelsPaint = new SolidColorPaint()
            {
                Color = SKColors.Gray,
                FontFamily = "Jetbrains Mono",
            },
            Labeler = _ => ConvertBytesHelper.ConvertBytes(_),
            SeparatorsPaint = new SolidColorPaint(SKColors.Gray) {
                StrokeThickness = 0.4f,
                PathEffect = new DashEffect(new float[] { 6, 6 }) } } };

    [ObservableProperty]
    private ObservableCollection<long> downloadSpeeds = new();

    [ObservableProperty]
    private ObservableCollection<long> uploadSpeeds = new();

    [ObservableProperty]
    public ISeries[] trafficSeries = Array.Empty<ISeries>();

    [ObservableProperty]
    public string? uploadTotal;

    [ObservableProperty]
    public string? downloadTotal;

    [ObservableProperty]
    public string? connectionsCount;

    public void OnNavigatedFrom()
    {

    }

    public void OnNavigatedTo()
    {
        if (!isInitialized)
            InitializeViewModel();
    }

    public static SKColor ResourcesToSKColor(string color, byte opacity = 255)
    {
        return new SKColor(
            ((Color)Application.Current.Resources[color]).R,
            ((Color)Application.Current.Resources[color]).G,
            ((Color)Application.Current.Resources[color]).B, opacity);
    }

    private void InitializeViewModel()
    {
        Task.Run(async () =>
        {
            await foreach (var _ in ClashService.GetRealTimeTraffic())
            {
                if (UploadSpeeds.Count >= 120)
                {
                    UploadSpeeds.RemoveAt(0);
                    DownloadSpeeds.RemoveAt(0);
                }

                UploadSpeeds.Add(_.Up);
                DownloadSpeeds.Add(_.Down);
            }
        });

        Task.Run(async () =>
        {
            await foreach (var _ in ClashService.GetRealTimeConnections())
            {
                UploadTotal = ConvertBytesHelper.ConvertBytes(_.UploadTotal);
                DownloadTotal = ConvertBytesHelper.ConvertBytes(_.DownloadTotal);
                ConnectionsCount = _.Connections.Count.ToString();
            }
        });

        TrafficSeries = new ISeries[] {
            new LineSeries<long> { Name = "上传",
                Values = UploadSpeeds, Stroke =
                new SolidColorPaint() { StrokeThickness = 0 },
                GeometrySize = 0, LineSmoothness = 0 },
            new LineSeries<long> { Name = "下载",
                Values = DownloadSpeeds, Stroke =
                new SolidColorPaint() { StrokeThickness = 0 },
                GeometrySize = 0, LineSmoothness = 0 } };

        isInitialized = true;
    }

    [RelayCommand]
    public void OnUpload()
    {
        TrafficSeries[0].IsVisible = !TrafficSeries[0].IsVisible;
    }

    [RelayCommand]
    public void OnDownload()
    {
        TrafficSeries[1].IsVisible = !TrafficSeries[1].IsVisible;
    }
}
