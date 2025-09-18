namespace EdAssistant.Models.Journal;

public class CommanderEvent : JournalEvent
{
    [JsonPropertyName("FID")]
    public string FId { get; set; }

    [JsonPropertyName("Name")]
    public string Name { get; set; }

    public override JournalEventTypeEnum EventTypeEnum => JournalEventTypeEnum.Commander;
}