using System.IO;
using System.Runtime.CompilerServices;

namespace ED.Assistant.Application.State;

class JournalStateApplier : IJournalStateApplier
{
	/// <summary>
	/// Snapshot-style or high-volume events that would only push
	/// meaningful entries out of the recent events feed.
	/// </summary>
	private static readonly HashSet<string> NotRecentEvents = new(StringComparer.OrdinalIgnoreCase)
	{
		CommanderEvent.EventName,
		MaterialsEvent.EventName,
		RankEvent.EventName,
		ShipLockerEvent.EventName,
		BaryCentreEvent.EventName,
		FSSSignalDiscoveredEvent.EventName
	};

	public Task ApplyFromFilesAsync(JournalState state, IEnumerable<string> filePaths,
		CancellationToken cancellationToken = default) => ApplyFromLinesAsync(state,
			ReadLinesFromFilesAsync(filePaths, cancellationToken), cancellationToken);

	public async Task ApplyFromLinesAsync(JournalState state, IAsyncEnumerable<string> lines,
		CancellationToken cancellationToken = default)
	{
		void ApplyLocation(LocationEvent e)
		{
			if (state.CurrentSystemAddress != e.SystemAddress)
			{
				ClearSystemData(state);
				state.FSDJump = null;
			}

			state.Location = e;
		}

		var dispatcher = new JournalEventDispatcher();
		var aggregator = new JournalStateAggregator(dispatcher);

		dispatcher.OnAny(e =>
		{
			state.LastEvent = e;

			if (!NotRecentEvents.Contains(e.Event))
				state.AddRecentEvent(e);
		});

		dispatcher.On<ScanOrganicEvent>(ScanOrganicEvent.EventName, e => state.Organics.Add(e));

		aggregator.RegisterLast<CommanderEvent>(
			CommanderEvent.EventName,
			e => state.Commander = e);

		aggregator.RegisterLast<LoadGameEvent>(
			LoadGameEvent.EventName,
			e => state.LoadGame = e);

		aggregator.RegisterLast<MaterialsEvent>(
			MaterialsEvent.EventName,
			e => state.Materials = e);

		aggregator.RegisterLast<RankEvent>(
			RankEvent.EventName,
			e => state.Ranks = e);

		aggregator.RegisterLast<ShipLockerEvent>(
			ShipLockerEvent.EventName,
			e => state.ShipLocker = e);

		aggregator.RegisterLast<FSDJumpEvent>(
			FSDJumpEvent.EventName,
			e =>
			{
				ClearSystemData(state);

				state.Location = null;
				state.FSDJump = e;
			});

		aggregator.RegisterByKey<ScanEvent, int>(
			ScanEvent.EventName,
			e => e.BodyId,
			state.Scans);

		aggregator.RegisterByKey<FSSBodySignalsEvent, int>(
			FSSBodySignalsEvent.EventName,
			e => e.BodyId,
			state.FSSSignals);

		aggregator.RegisterByKey<BaryCentreEvent, int>(
			BaryCentreEvent.EventName,
			e => e.BodyId,
			state.BaryCentres);

		aggregator.RegisterByKey<SAASignalsFoundEvent, int>(
			SAASignalsFoundEvent.EventName,
			e => e.BodyId,
			state.SAASignals);

		aggregator.RegisterByKey<FSSSignalDiscoveredEvent, string>(
			FSSSignalDiscoveredEvent.EventName,
			e => e.Key,
			state.SystemSignals);

		aggregator.RegisterLast<LocationEvent>(
			LocationEvent.EventName,
			ApplyLocation);

		aggregator.RegisterLast<LocationEvent>(
			"CarrierJump",
			ApplyLocation);

		await dispatcher.DispatchAsync(CaptureAsync(state.Log, lines, cancellationToken), cancellationToken);
	}
	
	private static async IAsyncEnumerable<string> CaptureAsync(JournalLog log, IAsyncEnumerable<string> lines,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		await foreach (var line in lines.WithCancellation(cancellationToken))
		{
			if (string.IsNullOrWhiteSpace(line))
				continue;

			log.Append(line);
			yield return line;
		}
	}

	private static void ClearSystemData(JournalState state)
	{
		state.Scans.Clear();
		state.FSSSignals.Clear();
		state.BaryCentres.Clear();
		state.Organics.Clear();
		state.SAASignals.Clear();
		state.SystemSignals.Clear();
	}

	private static async IAsyncEnumerable<string> ReadLinesFromFilesAsync(IEnumerable<string> filePaths,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		foreach (var filePath in filePaths)
		{
			await using var stream = new FileStream(filePath, new FileStreamOptions
			{
				Mode = FileMode.Open,
				Access = FileAccess.Read,
				Share = FileShare.ReadWrite,
				Options = FileOptions.Asynchronous | FileOptions.SequentialScan
			});

			using var reader = new StreamReader(stream);

			while (await reader.ReadLineAsync(cancellationToken) is { } line)
				yield return line;
		}
	}
}