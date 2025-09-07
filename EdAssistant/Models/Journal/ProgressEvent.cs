namespace EdAssistant.Models.Journal;

public sealed class ProgressEvent : JournalEventBase
{
    [JsonPropertyName("event")] public string? Event { get; set; }
    public int Combat { get; set; }
    public int Trade { get; set; }
    public int Explore { get; set; }
    public int Soldier { get; set; }
    public int Exobiologist { get; set; }
    public int Empire { get; set; }
    public int Federation { get; set; }
    public int CQC { get; set; }

    public override JournalEventType EventType => JournalEventType.Progress;
}