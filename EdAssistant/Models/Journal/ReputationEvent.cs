namespace EdAssistant.Models.Journal;

public class ReputationEvent : JournalEvent
{
    [JsonPropertyName("Empire")]
    public double Empire { get; set; }

    [JsonPropertyName("Federation")]
    public double Federation { get; set; }

    [JsonPropertyName("Independent")]
    public double Independent { get; set; }

    [JsonPropertyName("Alliance")]
    public double Alliance { get; set; }

    public override JournalEventTypeEnum EventTypeEnum => JournalEventTypeEnum.Reputation;
}