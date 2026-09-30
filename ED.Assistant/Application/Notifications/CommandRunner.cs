using System.Diagnostics;

namespace ED.Assistant.Application.Notifications;

static class CommandRunner
{
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
                Console.Error.WriteLine($"[notify] failed to start {fileName}");
                return;
            }

            _ = Task.Run(async () =>
            {
                using (process)
                {
                    var stdout = await process.StandardOutput.ReadToEndAsync();
                    var stderr = await process.StandardError.ReadToEndAsync();
                    await process.WaitForExitAsync();

                    Console.Error.WriteLine(
                        $"[notify] {fileName} {string.Join(' ', args)} -> exit {process.ExitCode} {stdout}{stderr}".Trim());
                }
            });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[notify] {fileName}: {ex.Message}");
        }
    }
}