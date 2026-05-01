namespace ED.Assistant.Data.Models.Events;

public class LoadGameEvent : IJournalEvent
{
	internal const string EventName = "LoadGame";

	public string Event => throw new NotImplementedException();

	public DateTime Timestamp => throw new NotImplementedException();
}
