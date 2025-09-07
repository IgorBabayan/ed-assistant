namespace EdAssistant.Models.Journal;

public sealed class UnknownEvent : JournalEventBase
{
    [JsonPropertyName("event")] public string? Event { get; set; }

    [JsonExtensionData] public Dictionary<string, JsonElement>? Extra { get; set; }

    public override JournalEventType EventType => JournalEventType.Unknown;
}