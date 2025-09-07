namespace EdAssistant.Models.Journal;

public sealed class ReputationEvent : JournalEventBase
{
    [JsonPropertyName("event")] public string? Event { get; set; }
    public double Empire { get; set; }
    public double Federation { get; set; }
    public double Independent { get; set; }
    public double Alliance { get; set; }

    public override JournalEventType EventType => JournalEventType.Reputation;
}