namespace ED.Assistant.Application.Path;

public static class PlatformPathResolverFactory
{
    public static IPlatformPathResolver Create()
    {
        if (OperatingSystem.IsWindows())
            return new WindowsPathResolver();

        if (OperatingSystem.IsLinux())
            return new LinuxPathResolver();

        return OperatingSystem.IsMacOS()
            ? new MacPathResolver()
            : throw new PlatformNotSupportedException("Unsupported OS");
    }
}