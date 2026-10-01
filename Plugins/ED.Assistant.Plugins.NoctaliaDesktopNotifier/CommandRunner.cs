using System.Diagnostics;

namespace ED.Assistant.Plugins.NoctaliaDesktopNotifier;

// Plugin copy of the host's CommandRunner (the host one isn't visible to plugins)
static class CommandRunner
{
    public static bool IsOnPath(string fileName) =>
        (Environment.GetEnvironmentVariable("PATH") ?? string.Empty)
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
            .Any(dir => File.Exists(Path.Combine(dir, fileName)));

    /// <summary>Starts the process and returns at once; output is logged in the background.</summary>
    public static void Run(string fileName, params string[] args)
    {
        try
        {
            var psi = new ProcessStartInfo(fileName)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            foreach (var arg in args)
                psi.ArgumentList.Add(arg);

            var process = Process.Start(psi);
            if (process is null)
            {
                Console.Error.WriteLine($"[noctalia] failed to start {fileName}");
                return;
            }

            _ = Task.Run(async () =>
            {
                using (process)
                {
                    var stdout = process.StandardOutput.ReadToEndAsync();
                    var stderr = process.StandardError.ReadToEndAsync();
                    await process.WaitForExitAsync();

                    Console.Error.WriteLine(
                        $"[noctalia] {fileName} {string.Join(' ', args)} -> exit {process.ExitCode} {await stdout}{await stderr}".Trim());
                }
            });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[noctalia] {fileName}: {ex.Message}");
        }
    }
}
