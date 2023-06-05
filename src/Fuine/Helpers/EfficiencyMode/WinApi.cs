namespace Fuine.Helpers.EfficiencyMode;

internal class WinApi
{
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetProcessInformation([In] nint hProcess,
        [In] PROCESS_INFORMATION_CLASS ProcessInformationClass, nint ProcessInformation, uint ProcessInformationSize);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetPriorityClass(nint handle, PriorityClass priorityClass);

    public enum PROCESS_INFORMATION_CLASS
    {
        ProcessMemoryPriority,
        ProcessMemoryExhaustionInfo,
        ProcessAppMemoryInfo,
        ProcessInPrivateInfo,
        ProcessPowerThrottling,
        ProcessReservedValue1,
        ProcessTelemetryCoverageInfo,
        ProcessProtectionLevelInfo,
        ProcessLeapSecondInfo,
        ProcessInformationClassMax,
    }

    [Flags]
    public enum ProcessorPowerThrottlingFlags : uint
    {
        None = 0x0,
        PROCESS_POWER_THROTTLING_EXECUTION_SPEED = 0x1,
    }

    public enum PriorityClass : uint
    {
        ABOVE_NORMAL_PRIORITY_CLASS = 0x8000,
        BELOW_NORMAL_PRIORITY_CLASS = 0x4000,
        HIGH_PRIORITY_CLASS = 0x80,
        IDLE_PRIORITY_CLASS = 0x40,
        NORMAL_PRIORITY_CLASS = 0x20,
        PROCESS_MODE_BACKGROUND_BEGIN = 0x100000,
        PROCESS_MODE_BACKGROUND_END = 0x200000,
        REALTIME_PRIORITY_CLASS = 0x100
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PROCESS_POWER_THROTTLING_STATE
    {
        public const uint PROCESS_POWER_THROTTLING_CURRENT_VERSION = 1;

        public uint Version;
        public ProcessorPowerThrottlingFlags ControlMask;
        public ProcessorPowerThrottlingFlags StateMask;
    }
}
