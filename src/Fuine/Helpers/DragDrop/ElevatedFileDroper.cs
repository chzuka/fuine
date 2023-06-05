namespace Fuine.Helpers.DragDrop;

public partial class ElevatedFileDroper
{
    public event EventHandler? Drop;
    public string[]? DropFilePaths { get; private set; }
    public POINT DropPoint { get; private set; }
    public HwndSource? HwndSource { get; set; }

    public void AddHook()
    {
        RemoveHook();
        HwndSource!.AddHook(WndProc);
        IntPtr handle = HwndSource.Handle;
        if (IsUserAnAdmin())
            RevokeDragDrop(handle);
        DragAcceptFiles(handle, true);
        ChangeMessageFilter(handle);
    }

    public void RemoveHook()
    {
        HwndSource!.RemoveHook(WndProc);
        DragAcceptFiles(HwndSource.Handle, false);
    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (TryGetDropInfo(msg, wParam, out string[]? filePaths, out POINT point))
        {
            DropPoint = point;
            DropFilePaths = filePaths;
            Drop?.Invoke(this, null!);
            handled = true;
        }

        return IntPtr.Zero;
    }
}
