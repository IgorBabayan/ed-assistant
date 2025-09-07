namespace EdAssistant.Models.Journal;

public sealed class EngineerProgressEvent : JournalEventBase
{
    public sealed class EngineerEntry
    {
        public string? Engineer { get; set; }
        public int EngineerID { get; set; }
        public string? Progress { get; set; }
        public int? RankProgress { get; set; }
        public int? Rank { get; set; }
    }

    [JsonPropertyName("event")] public string? Event { get; set; }
    [JsonPropertyName("Engineers")] public List<EngineerEntry>? Engineers { get; set; }

    public override JournalEventType EventType => JournalEventType.EngineerProgress;
}