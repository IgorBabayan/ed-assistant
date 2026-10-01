using System.Text;

namespace ED.Assistant.Plugins.Explorer.Journal;

internal static class JournalFiles
{
    /// <summary>Journal files, oldest first (same ordering as the host's watcher).</summary>
    public static IReadOnlyList<string> List(string folder) =>
        Directory.EnumerateFiles(folder, "Journal.*.log")
            .OrderBy(Path.GetFileName, StringComparer.Ordinal)
            .ToList();

    /// <summary>
    /// Reads complete lines from <paramref name="position"/>. A trailing partial line (the game
    /// is still writing it) is left for the next read, so the returned position always sits
    /// right after a newline.
    /// </summary>
    public static async Task<(List<string> Lines, long Position)> ReadCompleteLinesAsync(string path,
        long position, CancellationToken cancellationToken)
    {
        await using var stream = new FileStream(path, new FileStreamOptions
        {
            Mode = FileMode.Open,
            Access = FileAccess.Read,
            Share = FileShare.ReadWrite | FileShare.Delete,
            Options = FileOptions.Asynchronous | FileOptions.SequentialScan
        });

        // The file was replaced or truncated: start over
        if (position > stream.Length)
            position = 0;

        var length = (int)(stream.Length - position);
        if (length <= 0)
            return ([], position);

        stream.Seek(position, SeekOrigin.Begin);
        var buffer = new byte[length];
        await stream.ReadExactlyAsync(buffer, cancellationToken);

        var end = Array.LastIndexOf(buffer, (byte)'\n');
        if (end < 0)
            return ([], position);

        var lines = Encoding.UTF8.GetString(buffer, 0, end + 1)
            .Split('\n')
            .Select(l => l.TrimEnd('\r').TrimStart('\uFEFF'))
            .Where(l => l.Length > 0)
            .ToList();

        return (lines, position + end + 1);
    }
}
