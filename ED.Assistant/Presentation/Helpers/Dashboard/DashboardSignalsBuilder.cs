using System.Text.RegularExpressions;
using ED.Assistant.Presentation.ViewModels.Dashboard;

namespace ED.Assistant.Presentation.Helpers.Dashboard;

static partial class DashboardSignalsBuilder
{
	private const string SaaPrefix = "$SAA_SignalType_";

	private static readonly string[] SystemTagOrder =
	[
		SignalTags.USS,
		SignalTags.Combat,
		SignalTags.Resource,
		SignalTags.Station,
		SignalTags.Carrier,
		SignalTags.Megaship,
		SignalTags.Installation,
		SignalTags.Titan,
		SignalTags.Beacon,
		SignalTags.Signal
	];

	public static IReadOnlyList<DashboardSignalViewModel> Build(JournalState state, DateTime utcNow)
	{
		if (state.CurrentSystemAddress is not { } address)
			return [];

		var systemName = state.Location?.StarSystem ?? state.FSDJump?.StarSystem ?? string.Empty;

		return
		[
			.. BuildBodySignals(state, address, systemName),
			.. BuildSystemSignals(state, address, utcNow)
		];
	}

	/// <summary>
	/// One row per body and signal type. DSS results win over FSS results
	/// because they carry the detected genera.
	/// </summary>
	private static IEnumerable<DashboardSignalViewModel> BuildBodySignals(
		JournalState state, long address, string systemName)
	{
		var scans = state.Scans.Values
			.Where(s => s.SystemAddress == address)
			.ToDictionary(s => s.BodyId);

		var mapped = state.SAASignals.Values
			.Where(s => s.SystemAddress == address)
			.ToDictionary(s => s.BodyId);

		var detected = state.FSSSignals.Values
			.Where(s => s.SystemAddress == address)
			.ToDictionary(s => s.BodyId);

		var rows = new List<(double Distance, string Body, DashboardSignalViewModel Row)>();

		foreach (var bodyId in mapped.Keys.Union(detected.Keys))
		{
			var saa = mapped.GetValueOrDefault(bodyId);
			var fss = detected.GetValueOrDefault(bodyId);

			var bodyName = saa?.BodyName ?? fss?.BodyName ?? $"Body {bodyId}";
			var signals = saa?.Signals ?? fss?.Signals ?? [];

			double? distance = scans.TryGetValue(bodyId, out var scan)
				? scan.DistanceFromArrivalLS
				: null;

			foreach (var signal in signals.Where(s => s.Count > 0))
			{
				var (label, tag) = DescribeBodySignal(signal);

				var detail = signal.Count == 1 ? "1 signal" : $"{signal.Count} signals";

				if (tag == SignalTags.Biological && saa?.Genuses is { } genuses)
				{
					var names = genuses
						.Select(g => Text(g.Genus, g.GenusId))
						.Distinct()
						.ToArray();

					if (names.Length > 0)
						detail += $": {string.Join(", ", names)}";
				}

				rows.Add((distance ?? double.MaxValue, bodyName, new DashboardSignalViewModel
				{
					Type = label,
					Tag = tag,
					Name = ShortBodyName(bodyName, systemName),
					Detail = detail,
					Distance = distance is { } d ? $"{d:N0} ls" : "—"
				}));
			}
		}

		return rows
			.OrderBy(r => r.Distance)
			.ThenBy(r => r.Body, StringComparer.OrdinalIgnoreCase)
			.Select(r => r.Row);
	}

	private static IEnumerable<DashboardSignalViewModel> BuildSystemSignals(
		JournalState state, long address, DateTime utcNow)
	{
		var rows = new List<DashboardSignalViewModel>();

		foreach (var signal in state.SystemSignals.Values.Where(s => s.SystemAddress == address))
		{
			var details = new List<string>();

			if (signal.TimeRemaining is { } remaining && remaining > 0)
			{
				var left = signal.Timestamp.ToUniversalTime().AddSeconds(remaining) - utcNow;

				// Temporary signal already despawned.
				if (left <= TimeSpan.Zero)
					continue;

				details.Add(left.TotalHours >= 1
					? $"expires in {(int)left.TotalHours}h {left.Minutes}m"
					: $"expires in {Math.Max(1, left.Minutes)}m");
			}

			var tag = SystemTag(signal);
			var name = Text(signal.SignalName, signal.SignalNameId);

			if (tag == SignalTags.USS)
			{
				var ussType = Text(signal.USSType, signal.USSTypeId);
				if (!string.IsNullOrWhiteSpace(ussType) &&
					!string.Equals(ussType, name, StringComparison.OrdinalIgnoreCase))
					details.Insert(0, ussType);

				if (!string.IsNullOrWhiteSpace(signal.SpawningFaction))
					details.Add(signal.SpawningFaction);
			}
			else if (!string.IsNullOrWhiteSpace(signal.SignalKind))
			{
				details.Insert(0, Humanize(signal.SignalKind));
			}

			rows.Add(new DashboardSignalViewModel
			{
				Type = SystemLabel(tag),
				Tag = tag,
				Name = name,
				Detail = string.Join(", ", details),
				ThreatLevel = tag is SignalTags.USS or SignalTags.Combat ? signal.ThreatLevel : null
			});
		}

		return rows
			.OrderBy(r => Array.IndexOf(SystemTagOrder, r.Tag))
			.ThenByDescending(r => r.ThreatLevel ?? -1)
			.ThenBy(r => r.Name, StringComparer.OrdinalIgnoreCase);
	}

	internal static (string Label, string Tag) DescribeBodySignal(SignalItem signal)
	{
		var id = signal.TypeId;

		if (!id.StartsWith(SaaPrefix, StringComparison.OrdinalIgnoreCase))
		{
			// Ring hotspots report the mineral itself, e.g. "Painite", "LowTemperatureDiamond".
			return (Text(signal.Name, Humanize(id)), SignalTags.Hotspot);
		}

		var core = id[SaaPrefix.Length..].TrimEnd(';');

		var tag = core switch
		{
			SignalTags.Biological or
			SignalTags.Geological or
			SignalTags.Human or
			SignalTags.Thargoid or
			SignalTags.Guardian => core,
			_ => SignalTags.Other
		};

		return (Text(signal.Name, core), tag);
	}

	private static string SystemTag(FSSSignalDiscoveredEvent signal)
	{
		var kind = signal.SignalKind;
		var id = signal.SignalNameId;

		return kind switch
		{
			"USS" => SignalTags.USS,
			"Combat" => SignalTags.Combat,
			"ResourceExtraction" => SignalTags.Resource,
			"FleetCarrier" or "SquadronCarrier" => SignalTags.Carrier,
			"Megaship" or "StationMegaShip" => SignalTags.Megaship,
			"Installation" => SignalTags.Installation,
			"NavBeacon" => SignalTags.Beacon,
			"Titan" => SignalTags.Titan,
			"Outpost" => SignalTags.Station,
			_ when kind.StartsWith("Station", StringComparison.Ordinal) => SignalTags.Station,

			// Older journals have no SignalType, fall back to the name.
			_ when signal.IsStation => SignalTags.Station,
			_ when id.StartsWith("$USS", StringComparison.OrdinalIgnoreCase) => SignalTags.USS,
			_ when id.StartsWith("$Warzone", StringComparison.OrdinalIgnoreCase) => SignalTags.Combat,
			_ => SignalTags.Signal
		};
	}

	private static string SystemLabel(string tag) => tag switch
	{
		SignalTags.Resource => "RES",
		SignalTags.Beacon => "Nav beacon",
		_ => tag
	};

	private static string ShortBodyName(string bodyName, string systemName)
	{
		if (string.IsNullOrWhiteSpace(systemName) ||
			!bodyName.StartsWith(systemName + " ", StringComparison.OrdinalIgnoreCase))
			return bodyName;

		return bodyName[(systemName.Length + 1)..];
	}

	internal static string Text(string? localised, string raw) =>
		!string.IsNullOrWhiteSpace(localised)
			? localised
			: raw.Trim('$', ';').Replace('_', ' ');

	private static string Humanize(string value) => CamelCaseBoundary().Replace(value, " ");

	[GeneratedRegex("(?<=[a-z])(?=[A-Z])")]
	private static partial Regex CamelCaseBoundary();
}