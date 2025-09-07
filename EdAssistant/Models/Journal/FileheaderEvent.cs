namespace EdAssistant.Models.Journal;

public sealed class FileheaderEvent : JournalEventBase
{
    [JsonPropertyName("event")] public string? Event { get; set; }
    [JsonPropertyName("part")] public int? Part { get; set; }
    [JsonPropertyName("language")] public string? Language { get; set; }
    [JsonPropertyName("Odyssey")] public bool? Odyssey { get; set; }
    [JsonPropertyName("gameversion")] public string? GameVersion { get; set; }
    [JsonPropertyName("build")] public string? Build { get; set; }

    public override JournalEventType EventType => JournalEventType.Fileheader;
}