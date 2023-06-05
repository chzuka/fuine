namespace Fuine.Helpers.Loopback;

public class LoopbackHelper
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SID_AND_ATTRIBUTES
    {
        public IntPtr Sid;
        public uint Attributes;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct INET_FIREWALL_AC_CAPABILITIES
    {
        public uint count;
        public IntPtr capabilities; //SID_AND_ATTRIBUTES
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct INET_FIREWALL_AC_BINARIES
    {
        public uint count;
        public IntPtr binaries;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct INET_FIREWALL_APP_CONTAINER
    {
        internal IntPtr appContainerSid;
        internal IntPtr userSid;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string appContainerName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string displayName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string description;
        internal INET_FIREWALL_AC_CAPABILITIES capabilities;
        internal INET_FIREWALL_AC_BINARIES binaries;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string workingDirectory;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string packageFullName;
    }

    // Call this API to free the memory returned by the Enumeration API 
    [DllImport("FirewallAPI.dll")]
    internal static extern void NetworkIsolationFreeAppContainers(IntPtr pACs);

    // Call this API to load the current list of LoopUtil-enabled AppContainers
    [DllImport("FirewallAPI.dll")]
    internal static extern uint NetworkIsolationGetAppContainerConfig(out uint pdwCntACs, out IntPtr appContainerSids);

    // Call this API to set the LoopUtil-exemption list 
    [DllImport("FirewallAPI.dll")]
    private static extern uint NetworkIsolationSetAppContainerConfig(uint pdwCntACs, SID_AND_ATTRIBUTES[] appContainerSids);

    // Use this API to convert a string SID into an actual SID 
    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    internal static extern bool ConvertStringSidToSid(string strSid, out IntPtr pSid);

    [DllImport("advapi32", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool ConvertSidToStringSid(IntPtr pSid, out string strSid);

    // Call this API to enumerate all of the AppContainers on the system 
    [DllImport("FirewallAPI.dll")]
    internal static extern uint NetworkIsolationEnumAppContainers(uint Flags, out uint pdwCntPublicACs, out IntPtr ppACs);

    //        DWORD NetworkIsolationEnumAppContainers(
    //  _In_   DWORD Flags,
    //  _Out_  DWORD *pdwNumPublicAppCs,
    //  _Out_  PINET_FIREWALL_APP_CONTAINER *ppPublicAppCs
    //);

    //http://msdn.microsoft.com/en-gb/library/windows/desktop/hh968116.aspx
    private enum NETISO_FLAG
    {
        NETISO_FLAG_FORCE_COMPUTE_BINARIES = 0x1,
        NETISO_FLAG_MAX = 0x2
    }

    public class AppContainer
    {
        public string AppContainerName { get; set; }
        public string DisplayName { get; set; }
        public string WorkingDirectory { get; set; }
        public string StringSid { get; set; }
        public List<uint>? Capabilities { get; set; }
        public bool LoopUtil { get; set; }

        public AppContainer(string appContainerName, string displayName, string workingDirectory, IntPtr Sid)
        {
            AppContainerName = appContainerName;
            DisplayName = displayName;
            WorkingDirectory = workingDirectory;
            ConvertSidToStringSid(Sid, out string tempSid);
            StringSid = tempSid;
        }
    }

    internal List<INET_FIREWALL_APP_CONTAINER>? AppList;
    internal List<SID_AND_ATTRIBUTES>? AppListConfig;
    public List<AppContainer> Apps = new();
    internal IntPtr _pACs;

    public LoopbackHelper()
    {
        LoadApps();
    }

    public void LoadApps()
    {
        Apps.Clear();
        _pACs = IntPtr.Zero;
        //Full List of Apps
        AppList = PI_NetworkIsolationEnumAppContainers();
        //List of Apps that have LoopUtil enabled.
        AppListConfig = PI_NetworkIsolationGetAppContainerConfig();
        foreach (var PI_app in AppList)
        {
            AppContainer app = new(PI_app.appContainerName, PI_app.displayName, PI_app.workingDirectory, PI_app.appContainerSid);
            GetCapabilites(PI_app.capabilities);

            app.LoopUtil = CheckLoopback(PI_app.appContainerSid);
            Apps.Add(app);
        }
    }
    private bool CheckLoopback(IntPtr intPtr)
    {
        foreach (SID_AND_ATTRIBUTES item in AppListConfig!)
        {
            ConvertSidToStringSid(item.Sid, out string left);
            ConvertSidToStringSid(intPtr, out string right);

            if (left == right)
            {
                return true;
            }
        }

        return false;
    }

    private static List<SID_AND_ATTRIBUTES> GetCapabilites(INET_FIREWALL_AC_CAPABILITIES cap)
    {
        List<SID_AND_ATTRIBUTES> mycap = new();

        IntPtr arrayValue = cap.capabilities;

        var structSize = Marshal.SizeOf(typeof(SID_AND_ATTRIBUTES));
        for (var i = 0; i < cap.count; i++)
        {
            var cur = (SID_AND_ATTRIBUTES)Marshal.PtrToStructure(arrayValue, typeof(SID_AND_ATTRIBUTES))!;
            mycap.Add(cur);
            arrayValue = new IntPtr(arrayValue + (long)structSize);
        }

        return mycap;
    }

    private static List<SID_AND_ATTRIBUTES> PI_NetworkIsolationGetAppContainerConfig()
    {

        IntPtr arrayValue = IntPtr.Zero;
        uint size = 0;
        var list = new List<SID_AND_ATTRIBUTES>();

        // Pin down variables
        GCHandle handle_pdwCntPublicACs = GCHandle.Alloc(size, GCHandleType.Pinned);
        GCHandle handle_ppACs = GCHandle.Alloc(arrayValue, GCHandleType.Pinned);
        NetworkIsolationGetAppContainerConfig(out size, out arrayValue);

        var structSize = Marshal.SizeOf(typeof(SID_AND_ATTRIBUTES));
        for (var i = 0; i < size; i++)
        {
            var cur = (SID_AND_ATTRIBUTES)Marshal.PtrToStructure(arrayValue, typeof(SID_AND_ATTRIBUTES))!;
            list.Add(cur);
            arrayValue = new IntPtr(arrayValue + (long)structSize);
        }

        //release pinned variables.
        handle_pdwCntPublicACs.Free();
        handle_ppACs.Free();

        return list;
    }

    private List<INET_FIREWALL_APP_CONTAINER> PI_NetworkIsolationEnumAppContainers()
    {

        IntPtr arrayValue = IntPtr.Zero;
        uint size = 0;
        var list = new List<INET_FIREWALL_APP_CONTAINER>();

        // Pin down variables
        GCHandle handle_pdwCntPublicACs = GCHandle.Alloc(size, GCHandleType.Pinned);
        GCHandle handle_ppACs = GCHandle.Alloc(arrayValue, GCHandleType.Pinned);

        NetworkIsolationEnumAppContainers((int)NETISO_FLAG.NETISO_FLAG_MAX, out size, out arrayValue);
        _pACs = arrayValue; //store the pointer so it can be freed when we close the form

        var structSize = Marshal.SizeOf(typeof(INET_FIREWALL_APP_CONTAINER));
        for (var i = 0; i < size; i++)
        {
            var cur = (INET_FIREWALL_APP_CONTAINER)Marshal.PtrToStructure(arrayValue, typeof(INET_FIREWALL_APP_CONTAINER))!;
            list.Add(cur);
            arrayValue = new IntPtr(arrayValue + (long)structSize);
        }

        //release pinned variables.
        handle_pdwCntPublicACs.Free();
        handle_ppACs.Free();

        return list;
    }

    public bool SaveLoopbackState()
    {
        var countEnabled = CountEnabledLoopUtil();
        SID_AND_ATTRIBUTES[] arr = new SID_AND_ATTRIBUTES[countEnabled];
        int count = 0;

        for (int i = 0; i < Apps.Count; i++)
        {
            if (Apps[i].LoopUtil)
            {
                arr[count].Attributes = 0;
                ConvertStringSidToSid(Apps[i].StringSid, out nint ptr);
                arr[count].Sid = ptr;
                count++;
            }
        }

        return NetworkIsolationSetAppContainerConfig((uint)countEnabled, arr) == 0;
    }

    private int CountEnabledLoopUtil()
    {
        var count = 0;
        for (int i = 0; i < Apps.Count; i++)
        {
            if (Apps[i].LoopUtil)
            {
                count++;
            }
        }

        return count;
    }

    public void FreeResources()
    {
        NetworkIsolationFreeAppContainers(_pACs);
    }

    public static void Filter(bool enabled)
    {
        LoopbackHelper loop = new();
        List<AppContainer> appFiltered = new();

        appFiltered.Clear();

        foreach (var app in loop.Apps)
        {
            app.LoopUtil = enabled;

            appFiltered.Add(app);
        }

        loop.SaveLoopbackState();
        loop.FreeResources();
    }
}
