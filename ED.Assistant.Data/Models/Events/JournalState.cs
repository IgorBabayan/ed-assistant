namespace ED.Assistant.Data.Models.Events;

public sealed class JournalState
{
	public CommanderEvent? Commander { get; set; }
	public LoadGameEvent? LoadGame { get; set; }
	public MaterialsEvent? Materials { get; set; }
	public RankEvent? Ranks { get; set; }
}
