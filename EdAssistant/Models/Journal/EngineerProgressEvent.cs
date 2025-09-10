namespace EdAssistant.Models.Journal;

public class EngineerProgressEvent : JournalEvent
{
    [JsonPropertyName("Engineers")]
    public List<Engineer> Engineers { get; set; } = new();

    public override JournalEventType EventType => JournalEventType.EngineerProgress;
}