namespace Fuine.Helpers.EfficiencyMode;

public static class EnergyManager
{
    private static readonly nint pThrottleOn = nint.Zero;
    private static readonly nint pThrottleOff = nint.Zero;
    private static readonly int szControlBlock = 0;

    static EnergyManager()
    {
        szControlBlock = Marshal.SizeOf<WinApi.PROCESS_POWER_THROTTLING_STATE>();
        pThrottleOn = Marshal.AllocHGlobal(szControlBlock);
        pThrottleOff = Marshal.AllocHGlobal(szControlBlock);

        var throttleState = new WinApi.PROCESS_POWER_THROTTLING_STATE
        {
            Version = WinApi.PROCESS_POWER_THROTTLING_STATE.PROCESS_POWER_THROTTLING_CURRENT_VERSION,
            ControlMask = WinApi.ProcessorPowerThrottlingFlags.PROCESS_POWER_THROTTLING_EXECUTION_SPEED,
            StateMask = WinApi.ProcessorPowerThrottlingFlags.PROCESS_POWER_THROTTLING_EXECUTION_SPEED,
        };

        var unthrottleState = new WinApi.PROCESS_POWER_THROTTLING_STATE
        {
            Version = WinApi.PROCESS_POWER_THROTTLING_STATE.PROCESS_POWER_THROTTLING_CURRENT_VERSION,
            ControlMask = WinApi.ProcessorPowerThrottlingFlags.PROCESS_POWER_THROTTLING_EXECUTION_SPEED,
            StateMask = WinApi.ProcessorPowerThrottlingFlags.None,
        };

        Marshal.StructureToPtr(throttleState, pThrottleOn, false);
        Marshal.StructureToPtr(unthrottleState, pThrottleOff, false);
    }

    public static void ToggleEfficiencyMode(nint hProcess, bool enable)
    {
        WinApi.SetProcessInformation(hProcess, WinApi.PROCESS_INFORMATION_CLASS.ProcessPowerThrottling,
            enable ? pThrottleOn : pThrottleOff, (uint)szControlBlock);
        WinApi.SetPriorityClass(hProcess, enable ? WinApi.PriorityClass.IDLE_PRIORITY_CLASS : WinApi.PriorityClass.NORMAL_PRIORITY_CLASS);
    }
}
