using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using ED.Assistant.Application.Evaluation;

namespace ED.Assistant.Application.JournalLoading;

sealed class JournalWatchService : IJournalWatchService
{
	// Changed events arrive in bursts (several per write, dozens per second while scanning).
	// Wait this long after the first one, then read everything new in a single pass.
	private static readonly TimeSpan ReadDelay = TimeSpan.FromMilliseconds(150);

	private readonly IJournalStateStore _stateStore;
	private readonly IJournalStateApplier _journalStateApplier;
	private readonly IEvaluatorSyncService _evaluatorSync;
	private readonly SemaphoreSlim _gate;

	private FileSystemWatcher? _watcher;
	private string? _currentFile;
	private long _position;
	private int _readScheduled;

	public JournalWatchService(IJournalStateStore stateStore, IJournalStateApplier journalStateApplier,
		IEvaluatorSyncService evaluatorSync)
	{
		_stateStore = stateStore;
		_journalStateApplier = journalStateApplier;
		_evaluatorSync = evaluatorSync;

		_gate = new(1, 1);
	}

	public Task StartAsync(string logFolder, CancellationToken cancellationToken = default)
	{
		Stop();

		if (!Directory.Exists(logFolder))
			throw new DirectoryNotFoundException(logFolder);

		_currentFile = GetLatestLogFile(logFolder);

		if (_currentFile is not null)
			_position = new FileInfo(_currentFile).Length;

		_watcher = new FileSystemWatcher(logFolder, "Journal.*.log")
		{
			NotifyFilter =
				NotifyFilters.FileName |
				NotifyFilters.LastWrite |
				NotifyFilters.Size,

			EnableRaisingEvents = false
		};

		_watcher.Changed += OnChanged;
		_watcher.Created += OnCreated;
		_watcher.EnableRaisingEvents = true;

		return Task.CompletedTask;
	}

	public void Dispose()
	{
		Stop();
		_gate.Dispose();
	}

	public void Stop()
	{
		if (_watcher is null)
			return;

		_watcher.EnableRaisingEvents = false;

		_watcher.Changed -= OnChanged;
		_watcher.Created -= OnCreated;

		_watcher.Dispose();
		_watcher = null;

		_currentFile = null;
		_position = 0;
	}

	private void OnChanged(object sender, FileSystemEventArgs e)
	{
		if (!IsCurrentFile(e.FullPath))
			return;

		// Trailing-edge debounce: only one read is scheduled at a time, and it runs
		// after the burst, so the last write of a burst is never missed.
		if (Interlocked.Exchange(ref _readScheduled, 1) == 1)
			return;

		_ = ReadAfterDelayAsync(e.FullPath);
	}

	private void OnCreated(object sender, FileSystemEventArgs e) => _ = RunSafeAsync(() => OnCreatedAsync(e.FullPath));

	private async Task ReadAfterDelayAsync(string path)
	{
		try
		{
			await Task.Delay(ReadDelay);

			// Cleared before reading: a write that lands during the read schedules another pass
			Volatile.Write(ref _readScheduled, 0);

			await _gate.WaitAsync();
			try
			{
				if (IsCurrentFile(path))
					await ReadNewLinesAsync(path);
			}
			finally
			{
				_gate.Release();
			}
		}
		catch (ObjectDisposedException)
		{
			// Service disposed while a read was pending
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Journal read failed: {ex}");
		}
	}

	private async Task OnCreatedAsync(string path)
	{
		if (!IsJournalFile(path))
			return;

		if (!IsNewerLogFile(path))
			return;

		await _gate.WaitAsync();
		try
		{
			_currentFile = path;
			_position = 0;

			await ReadNewLinesAsync(path);
		}
		finally
		{
			_gate.Release();
		}
	}

	private static async Task RunSafeAsync(Func<Task> action)
	{
		try
		{
			await action();
		}
		catch (ObjectDisposedException)
		{
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Journal watch failed: {ex}");
		}
	}

	private async Task ReadNewLinesAsync(string path, CancellationToken cancellationToken = default)
	{
		var state = _stateStore.CurrentState;
		if (state is null)
			return;

		if (!File.Exists(path))
			return;

		var lines = ReadNewLinesFromCurrentFileAsync(path, cancellationToken);
		await _journalStateApplier.ApplyFromLinesAsync(state, lines, cancellationToken);
		await _evaluatorSync.SyncAsync(state, cancellationToken);

		_stateStore.Update(state);
	}

	private async IAsyncEnumerable<string> ReadNewLinesFromCurrentFileAsync(string path,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		await using var stream = new FileStream(path, new FileStreamOptions
		{
			Mode = FileMode.Open,
			Access = FileAccess.Read,
			Share = FileShare.ReadWrite,
			Options = FileOptions.Asynchronous | FileOptions.SequentialScan
		});

		if (_position > stream.Length)
			_position = 0;

		stream.Seek(_position, SeekOrigin.Begin);

		using var reader = new StreamReader(stream);

		while (await reader.ReadLineAsync(cancellationToken) is { } line)
		{
			if (!string.IsNullOrWhiteSpace(line))
				yield return line;
		}

		_position = stream.Position;
	}

	private bool IsCurrentFile(string path) => string.Equals(IOPath.GetFullPath(path),
		IOPath.GetFullPath(_currentFile ?? string.Empty), StringComparison.OrdinalIgnoreCase);

	private bool IsNewerLogFile(string path)
	{
		if (_currentFile is null)
			return true;

		var currentName = IOPath.GetFileName(_currentFile);
		var newName = IOPath.GetFileName(path);

		return string.Compare(newName, currentName, StringComparison.OrdinalIgnoreCase) > 0;
	}

	private static bool IsJournalFile(string path)
	{
		var fileName = IOPath.GetFileName(path);
		return fileName.StartsWith("Journal.", StringComparison.OrdinalIgnoreCase)
			&& fileName.EndsWith(".log", StringComparison.OrdinalIgnoreCase);
	}

	private static string? GetLatestLogFile(string logFolder) => Directory.GetFiles(logFolder, "Journal.*.log")
		.OrderByDescending(File.GetLastWriteTimeUtc)
		.FirstOrDefault();
}