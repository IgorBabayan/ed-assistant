namespace ED.Assistant.Application.Notifications;

static class DesktopNotifierFactory
{
    public static IDesktopNotifier Create()
    {
#if WINDOWS
        if (OperatingSystem.IsWindows())
            return new WindowsDesktopNotifier();
#endif
        if (OperatingSystem.IsMacOS())
            return new MacDesktopNotifier();

        if (OperatingSystem.IsLinux())
            return HyprlandDesktopNotifier.IsRunning
                ? new HyprlandDesktopNotifier()
                : new LinuxDesktopNotifier();

        return new NullDesktopNotifier();
    }
}