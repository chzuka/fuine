using Microsoft.Win32.TaskScheduler;

using Trigger = Microsoft.Win32.TaskScheduler.Trigger;

namespace Fuine.Services;
internal class TaskSchedulerService
{
    public static void AddTask(string name, string trigger)
    {
        using TaskService service = new();
        using var task = service.GetTask(name);

        TaskDefinition definition;

        if (task == null)
        {
            definition = TaskService.Instance.NewTask();

            definition.Settings.DisallowStartIfOnBatteries = false;
            definition.Settings.IdleSettings.StopOnIdleEnd = false;
            definition.Settings.RunOnlyIfNetworkAvailable = true;
            definition.Settings.StopIfGoingOnBatteries = false;
            definition.Settings.RunOnlyIfIdle = false;
            definition.Settings.StartWhenAvailable = true;

            definition.Settings.Hidden = true;
            definition.Settings.UseUnifiedSchedulingEngine = true;
            definition.Settings.DisallowStartOnRemoteAppSession = false;

            definition.Settings.Compatibility = TaskCompatibility.V2_3;
            definition.Settings.ExecutionTimeLimit = TimeSpan.Zero;

            definition.Actions.Add(Path.Combine(Global.应用目录, "Fuine.exe"), "-up");
        }
        else
        {
            definition = task.Definition;
        }


        if (definition.Triggers.Any(_ => _.TriggerType == (
        trigger == "start" ? AutoStartTrigger().TriggerType : AutoUpdateTrigger().TriggerType)))
        {
            return;
        }

        definition.Triggers.Add(trigger == "start" ?
            AutoStartTrigger() : AutoUpdateTrigger());

        TaskService.Instance.RootFolder.RegisterTaskDefinition(name, definition);
    }

    public static void RemoveTask(string name, string trigger)
    {
        using TaskService service = new();
        using var task = service.GetTask(name);

        if (task == null)
        {
            return;
        }


        task.Definition.Triggers.RemoveMany(
            task.Definition.Triggers.
            Where(_ => _.TriggerType == (
            trigger == "start" ?
            AutoStartTrigger().TriggerType :
            AutoUpdateTrigger().TriggerType)));

        task.RegisterChanges();

        if (task.Definition.Triggers.Count == 0)
        {
            service.RootFolder.DeleteFolder(name);
        }
    }

    public static void RemoveTask(string name)
    {
        using TaskService service = new();
        using var task = service.GetTask(name);

        if (task == null)
        {
            return;
        }

        service.RootFolder.DeleteFolder(name);
    }

    private static Trigger AutoStartTrigger()
    {
        var user = WindowsIdentity.GetCurrent().Name;
        return new LogonTrigger { UserId = user };
    }

    private static Trigger AutoUpdateTrigger()
    {
        return new TimeTrigger
        {
            Repetition = { Interval = TimeSpan.FromHours(2) },
            ExecutionTimeLimit = TimeSpan.FromMinutes(5),
        };
    }

    public static void RunTaskScheduler(string name)
    {
        using TaskService service = new();
        service.GetTask(name)?.Run();
    }
}
