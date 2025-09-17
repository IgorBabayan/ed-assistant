namespace EdAssistant.Models.Journal;

public class FileHeaderEvent : JournalEvent
{
    [JsonPropertyName("part")]
    public int Part { get; set; }

    [JsonPropertyName("language")]
    public required string Language { get; set; }

    [JsonPropertyName("Odyssey")]
    public bool Odyssey { get; set; }

    [JsonPropertyName("gameversion")]
    public required string GameVersion { get; set; }

    [JsonPropertyName("build")]
    public required string Build { get; set; }

    public override JournalEventType EventType => JournalEventType.FileHeader;
}