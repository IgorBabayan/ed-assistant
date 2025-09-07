namespace EdAssistant.Models.Journal;

public sealed class PowerplayEvent : JournalEventBase
{
    [JsonPropertyName("event")] public string? Event { get; set; }
    public string? Power { get; set; }
    public int Rank { get; set; }
    public int Merits { get; set; }
    public long TimePledged { get; set; }

    public override JournalEventType EventType => JournalEventType.Powerplay;
}