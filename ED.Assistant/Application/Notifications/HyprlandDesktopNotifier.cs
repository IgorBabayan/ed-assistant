namespace ED.Assistant.Application.Notifications;

sealed class HyprlandDesktopNotifier : IDesktopNotifier
{
    private const string Icon = "1";
    private const string DurationMs = "6000";
    private const string Color = "rgb(66ee99)";

    public static bool IsRunning =>
        !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("HYPRLAND_INSTANCE_SIGNATURE"));

    public void Show(string title, string message) =>
        CommandRunner.Run("hyprctl", "notify", Icon, DurationMs, Color, $"{title}: {message}");
}