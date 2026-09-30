using System.IO;
using System.Runtime.CompilerServices;
using ED.Assistant.Domain.Types;

namespace ED.Assistant.Application.State;

internal class JournalStateApplier : IJournalStateApplier
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
		// Used through their interfaces: the applier only depends on the dispatch/aggregate contracts
		IJournalEventDispatcher dispatcher = new JournalEventDispatcher();
		IJournalStateAggregator aggregator = new JournalStateAggregator(dispatcher);

		dispatcher.OnAny(e =>
		{
			state.LastEvent = e;

			if (!NotRecentEvents.Contains(e.Event))
				state.AddRecentEvent(e);
		});

		dispatcher.On<ScanOrganicEvent>(ScanOrganicEvent.EventName, e =>
		{
			if (IsFirstSample(state, e))
			{
				state.PendingEvaluatorChanges.Add(new OrganicSampled(
					e.Timestamp, e.SystemAddress, e.BodyId, e.SpeciesId, HasFirstFootStep(state, e)));
			}

			state.Organics.Add(e);
		});

		dispatcher.On<SellOrganicDataEvent>(SellOrganicDataEvent.EventName, e =>
			state.PendingEvaluatorChanges.Add(new OrganicDataSold(
				e.Timestamp,
				[..(e.BioData ?? []).Select(b => b.SpeciesId)])));

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

		aggregator.RegisterByKey(
			ScanEvent.EventName,
			e => e.BodyId,
			state.Scans);

		aggregator.RegisterByKey(
			FSSBodySignalsEvent.EventName,
			e => e.BodyId,
			state.FSSSignals);

		aggregator.RegisterByKey(
			BaryCentreEvent.EventName,
			e => e.BodyId,
			state.BaryCentres);

		aggregator.RegisterByKey(
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
		return;

		void ApplyLocation(LocationEvent e)
		{
			if (state.CurrentSystemAddress != e.SystemAddress)
			{
				ClearSystemData(state);
				state.FSDJump = null;
			}

			state.Location = e;
		}
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
	
	private static bool IsFirstSample(JournalState state, ScanOrganicEvent e)
	{
		if (e.ScanType != ScanType.Sample)
			return false;

		var previous = state.Organics.LastOrDefault(o =>
			o.SystemAddress == e.SystemAddress &&
			o.BodyId == e.BodyId &&
			string.Equals(o.SpeciesId, e.SpeciesId, StringComparison.OrdinalIgnoreCase));

		return previous is null || previous.ScanType != ScanType.Sample;
	}

	private static bool HasFirstFootStep(JournalState state, ScanOrganicEvent e) =>
		state.Scans.TryGetValue(e.BodyId, out var scan) &&
		scan.SystemAddress == e.SystemAddress &&
		!scan.WasFootfalled;

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