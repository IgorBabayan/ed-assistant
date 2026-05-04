using System.IO;
using System.Runtime.CompilerServices;

namespace ED.Assistant.Data.Services.Events;

public interface IJournalStateApplier
{
	Task ApplyFromFilesAsync(JournalState state, IEnumerable<string> filePaths,
		CancellationToken cancellationToken = default);

	Task ApplyFromLinesAsync(JournalState state, IAsyncEnumerable<string> lines,
		CancellationToken cancellationToken = default);
}

class JournalStateApplier : IJournalStateApplier
{
	public Task ApplyFromFilesAsync(JournalState state, IEnumerable<string> filePaths,
		CancellationToken cancellationToken = default) => ApplyFromLinesAsync(state,
			ReadLinesFromFilesAsync(filePaths, cancellationToken), cancellationToken);

	public async Task ApplyFromLinesAsync(JournalState state, IAsyncEnumerable<string> lines,
		CancellationToken cancellationToken = default)
	{
		var dispatcher = new JournalEventDispatcher();
		var aggregator = new JournalStateAggregator(dispatcher);

		dispatcher.OnAny(e => state.LastEvent = e);

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
				state.FSDJump = e;
				state.Scans.Clear();
				state.FSSSignals.Clear();
				state.BaryCentres.Clear();
				state.Organics.Clear();
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

		aggregator.RegisterByKey<ScanOrganicEvent, int>(
			ScanOrganicEvent.EventName,
			e => e.BodyId,
			state.Organics);

		aggregator.RegisterByKey<SAASignalsFoundEvent, int>(
			SAASignalsFoundEvent.EventName,
			e => e.BodyId,
			state.SAASignals);

		await dispatcher.DispatchAsync(lines, cancellationToken);
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
