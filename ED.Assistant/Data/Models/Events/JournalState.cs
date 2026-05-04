namespace ED.Assistant.Data.Models.Events;

public sealed class JournalState
{
	public string? FileName { get; init; }
	public IJournalEvent? LastEvent { get; set; }

	public CommanderEvent? Commander { get; set; }
	public LoadGameEvent? LoadGame { get; set; }
	public MaterialsEvent? Materials { get; set; }
	public RankEvent? Ranks { get; set; }
	public FSDJumpEvent? FSDJump { get; set; }
	public ShipLockerEvent? ShipLocker { get; set; }

	public Dictionary<int, ScanEvent> Scans { get; } = new();
	public Dictionary<int, ScanOrganicEvent> Organics { get; } = new();
	public Dictionary<int, SAASignalsFoundEvent> SAASignals { get; } = new();
	public Dictionary<int, BaryCentreEvent> BaryCentres { get; } = new();
	public Dictionary<int, FSSBodySignalsEvent> FSSSignals { get; } = new();
}
