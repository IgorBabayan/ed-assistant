namespace EdAssistant.Models.Journal;

public class CommanderEvent : JournalEvent
{
    [JsonPropertyName("FID")]
    public string FID { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; }

    public override JournalEventType EventType => JournalEventType.Commander;
}