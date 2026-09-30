using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Channels;
using ED.Assistant.Application.Evaluation;
using ED.Assistant.Application.Notifications;

namespace ED.Assistant.Application.JournalLoading;

internal sealed class JournalWatchService : IJournalWatchService, IAsyncDisposable
{
	private static readonly TimeSpan ReadDelay = TimeSpan.FromMilliseconds(150);
	private readonly IJournalStateStore _stateStore;
	private readonly IJournalStateApplier _journalStateApplier;
	private readonly IEvaluatorSyncService _evaluatorSync;
	private readonly BioSignalAlerter _alertService;
	
	private FileSystemWatcher? _watcher;
	private CancellationTokenSource? _cancellation;
	private Task _worker = Task.CompletedTask;
	
	public bool IsRunning => _watcher is not null;

	public JournalWatchService(IJournalStateStore stateStore, IJournalStateApplier journalStateApplier,
		IEvaluatorSyncService evaluatorSync, BioSignalAlerter bioAlerter, BioSignalAlerter alertService)
	{
		_stateStore = stateStore;
		_journalStateApplier = journalStateApplier;
		_evaluatorSync = evaluatorSync;
		_alertService = alertService;
	}

	public async Task StartAsync(string logFolder, CancellationToken cancellationToken = default)
	{
		await StopAsync();
		cancellationToken.ThrowIfCancellationRequested();
		if (!Directory.Exists(logFolder))
			throw new DirectoryNotFoundException(logFolder);

		var changes = Channel.CreateBounded<bool>(new BoundedChannelOptions(1)
		{
			FullMode = BoundedChannelFullMode.DropWrite,
			SingleReader = true
		});
		var currentFile = GetLatestLogFile(logFolder);
		var position = currentFile is null ? 0 : FindLastLineEnd(currentFile);
		var watcher = new FileSystemWatcher(logFolder, "Journal.*.log")
		{
			NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size
		};
		watcher.Changed += (_, _) => changes.Writer.TryWrite(true);
		watcher.Created += (_, _) => changes.Writer.TryWrite(true);
		watcher.Renamed += (_, _) => changes.Writer.TryWrite(true);
		watcher.Error += (_, e) =>
		{
			Debug.WriteLine($"Journal watcher error: {e.GetException()}");
			changes.Writer.TryWrite(true);
		};

		try
		{
			watcher.EnableRaisingEvents = true;
		}
		catch
		{
			watcher.Dispose();
			throw;
		}

		_watcher = watcher;
		_cancellation = new CancellationTokenSource();
		var token = _cancellation.Token;
		_worker = Task.Run(() => WatchAsync(logFolder, currentFile, position, changes.Reader, token), token);
		// Catch writes/rotation between taking the initial position and enabling the watcher.
		changes.Writer.TryWrite(true);
	}

	private void Stop()
	{
		_watcher?.Dispose();
		_watcher = null;
		_cancellation?.Cancel();
	}

	public async Task StopAsync()
	{
		Stop();
		try
		{
			await _worker.ConfigureAwait(false);
		}
		catch (OperationCanceledException)
		{
		}
		finally
		{
			_cancellation?.Dispose();
			_cancellation = null;
		}
	}

	public void Dispose() => StopAsync().GetAwaiter().GetResult();
	public async ValueTask DisposeAsync() => await StopAsync().ConfigureAwait(false);

	private async Task WatchAsync(string folder, string? currentFile, long position,
		ChannelReader<bool> changes, CancellationToken cancellationToken)
	{
		// Only "something changed" matters, not the values: wait for a signal, let the writes
		// settle, then drain every signal queued meanwhile (including the one that woke us)
		while (await changes.WaitToReadAsync(cancellationToken))
		{
			await Task.Delay(ReadDelay, cancellationToken);
			while (changes.TryRead(out _)) { }

			try
			{
				var files = Directory.EnumerateFiles(folder, "Journal.*.log")
                    .OrderBy(IOPath.GetFileName, StringComparer.Ordinal).ToArray();
                foreach (var path in files)
                {
                    if (currentFile is not null && string.CompareOrdinal(IOPath.GetFileName(path), IOPath.GetFileName(currentFile)) < 0)
                        continue;
                    if (!string.Equals(path, currentFile, StringComparison.Ordinal))
                    {
                        currentFile = path;
                        position = 0;
                    }
                    position = await ReadNewLinesAsync(path, position, cancellationToken);
                }
			}
			catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
			{
				throw;
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Journal read failed: {ex}");
			}
		}
	}

	private async Task<long> ReadNewLinesAsync(string path, long position, CancellationToken cancellationToken)
	{
		var (lines, nextPosition) = await ReadCompleteLinesAsync(path, position, cancellationToken);
		if (lines.Count == 0 && _stateStore.CurrentState.PendingEvaluatorChanges.Count == 0)
			return nextPosition;

		// Published states are never mutated by the watcher; readers can safely enumerate them.
		var state = _stateStore.CurrentState.CreateSnapshot();
		state.FileName = IOPath.GetFileName(path);
		await _journalStateApplier.ApplyFromLinesAsync(state, AsAsyncLines(lines, cancellationToken), cancellationToken);
		
		_alertService.Publish(state.PendingAlerts);
		state.PendingAlerts.Clear();

		
		try
		{
			await _evaluatorSync.SyncAsync(state, cancellationToken);
		}
		catch (Exception ex) when (ex is not OperationCanceledException)
		{
			Debug.WriteLine($"Evaluator sync failed; changes retained for retry: {ex}");
		}
		finally
		{
			// Preserve pending database changes for retry if sync failed.
			if (!cancellationToken.IsCancellationRequested)
				_stateStore.Update(state);
		}
		return nextPosition;
	}

	// Internal (not private) so ED.Assistant.Tests can exercise partial-line handling directly
	// ReSharper disable once MemberCanBePrivate.Global
	internal static async Task<(List<string> Lines, long Position)> ReadCompleteLinesAsync(
		string path, long position, CancellationToken cancellationToken = default)
	{
		await using var stream = new FileStream(path, new FileStreamOptions
		{
			Mode = FileMode.Open,
			Access = FileAccess.Read,
			Share = FileShare.ReadWrite | FileShare.Delete,
			Options = FileOptions.Asynchronous | FileOptions.SequentialScan
		});
		if (position > stream.Length)
			position = 0;
		stream.Seek(position, SeekOrigin.Begin);

		var lines = new List<string>();
		var buffer = new byte[8192];
		using var line = new MemoryStream();
		var offset = position;
		int read;
		while ((read = await stream.ReadAsync(buffer, cancellationToken)) > 0)
		{
			for (var i = 0; i < read; i++)
			{
				offset++;
				if (buffer[i] != (byte)'\n')
				{
					line.WriteByte(buffer[i]);
					continue;
				}

				var text = Encoding.UTF8.GetString(line.GetBuffer(), 0, (int)line.Length).TrimEnd('\r');
				if (position == 0)
					text = text.TrimStart('\uFEFF');
				if (!string.IsNullOrWhiteSpace(text))
					lines.Add(text);
				line.SetLength(0);
				position = offset;
			}
		}
		// Bytes after the last newline are read again once the game completes the record.
		return (lines, position);
	}

	private static async IAsyncEnumerable<string> AsAsyncLines(IEnumerable<string> lines,
		[System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken)
	{
		foreach (var line in lines)
		{
			cancellationToken.ThrowIfCancellationRequested();
			yield return line;
		}
		await Task.CompletedTask;
	}

    private static long FindLastLineEnd(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
        var buffer = new byte[4096];
        var end = stream.Length;
        while (end > 0)
        {
            var start = Math.Max(0, end - buffer.Length);
            stream.Position = start;
            var count = stream.Read(buffer, 0, (int)(end - start));
            for (var i = count - 1; i >= 0; i--)
                if (buffer[i] == (byte)'\n')
                    return start + i + 1;
            end = start;
        }
        return 0;
    }

	private static string? GetLatestLogFile(string folder) => Directory.EnumerateFiles(folder, "Journal.*.log")
		.OrderByDescending(IOPath.GetFileName, StringComparer.Ordinal)
		.FirstOrDefault();
}
