using System.Globalization;
using ED.Assistant.Presentation.ViewModels.Dashboard;

namespace ED.Assistant.Presentation.Helpers.Dashboard;

internal static class RecentEventsBuilder
{
	/// <summary>Newest first, only events that have a readable message.</summary>
	public static IReadOnlyList<RecentEventViewModel> Build(
		IReadOnlyList<IJournalEvent> events, int limit, DateTime localNow)
	{
		var result = new List<RecentEventViewModel>(limit);

		for (var i = events.Count - 1; i >= 0 && result.Count < limit; i--)
		{
			var journalEvent = events[i];

			if (Format(journalEvent) is not { } message)
				continue;

			var local = journalEvent.Timestamp.ToLocalTime();

			result.Add(new RecentEventViewModel
			{
				Time = local.Date == localNow.Date
					? local.ToString("HH:mm:ss", CultureInfo.InvariantCulture)
					: local.ToString("dd.MM HH:mm", CultureInfo.InvariantCulture),
				FullTime = local.ToString("F", CultureInfo.CurrentCulture),
				Message = message
			});
		}

		return result;
	}

	private static string? Format(IJournalEvent journalEvent) => journalEvent switch
	{
		FSDJumpEvent e => $"Jumped to {e.StarSystem} ({e.JumpDist:0.00} ly)",

		LocationEvent e when string.Equals(e.Event, "CarrierJump", StringComparison.OrdinalIgnoreCase)
			=> $"Carrier jumped to {e.StarSystem}",

		LocationEvent e => $"Located in {e.StarSystem}",

		LoadGameEvent e => string.IsNullOrWhiteSpace(e.ShipFullTitle)
			? "Game loaded"
			: $"Game loaded in {e.ShipFullTitle}",

		ScanEvent e => FormatScan(e),

		FSSBodySignalsEvent e => $"Signals detected on {e.BodyName}: {Summarize(e.Signals)}",

		SAASignalsFoundEvent e => Summarize(e.Signals) is { Length: > 0 } summary
			? $"Surface mapped: {e.BodyName} ({summary})"
			: $"Surface mapped: {e.BodyName}",

		ScanOrganicEvent e => FormatOrganic(e),

		_ => null
	};

	private static string? FormatScan(ScanEvent e)
	{
		string description;

		if (!string.IsNullOrWhiteSpace(e.StarType))
			description = $"{e.StarType}-class star";
		else if (!string.IsNullOrWhiteSpace(e.PlanetClass))
			description = e.PlanetClass;
		else
			return null; // Belt clusters and rings only add noise.

		var notes = new List<string> { description };

		if (string.Equals(e.TerraformState, "Terraformable", StringComparison.OrdinalIgnoreCase))
			notes.Add("terraformable");

		if (e.IsLandable)
			notes.Add("landable");

		if (!e.WasDiscovered)
			notes.Add("first discovery");

		return $"Scanned {e.BodyName}: {string.Join(", ", notes)}";
	}

	private static string FormatOrganic(ScanOrganicEvent e)
	{
		var verb = e.ScanType switch
		{
			"Log" => "Logged",
			"Sample" => "Sampled",
			"Analyse" => "Analysed",
			_ => $"Scanned ({e.ScanType})"
		};

		var name = DashboardSignalsBuilder.Text(
			string.IsNullOrWhiteSpace(e.Species) ? e.Genus : e.Species,
			string.IsNullOrWhiteSpace(e.SpeciesId) ? e.GenusId : e.SpeciesId);

		return $"{verb} {name}";
	}

	private static string Summarize(IEnumerable<SignalItem>? signals) => string.Join(", ",
		(signals ?? [])
			.Where(s => s.Count > 0)
			.Select(s => $"{s.Count} {DashboardSignalsBuilder.DescribeBodySignal(s).Label}"));
}