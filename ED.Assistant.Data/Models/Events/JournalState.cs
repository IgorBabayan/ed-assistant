namespace ED.Assistant.Data.Models.Events;

public sealed class JournalState
{
	public CommanderEvent? Commander { get; set; }
	public LoadGameEvent? LoadGame { get; set; }
	public MaterialsEvent? Materials { get; set; }
	public RankEvent? Ranks { get; set; }
	public FSDJumpEvent? FSDJump { get; set; }

	public Dictionary<int, ScanEvent> Scans { get; } = new();
	public Dictionary<int, ScanBaryCentreEvent> BaryCentres { get; } = new();
}
