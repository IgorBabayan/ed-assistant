namespace EdAssistant.Models.Journal;

public class CommanderEvent : JournalEvent
{
    [JsonPropertyName("FID")]
    public required string FId { get; set; }

    [JsonPropertyName("Name")]
    public required string Name { get; set; }

    public override JournalEventType EventType => JournalEventType.Commander;
}