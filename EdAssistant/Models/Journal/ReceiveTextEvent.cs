namespace EdAssistant.Models.Journal;

public sealed class ReceiveTextEvent : JournalEventBase
{
    [JsonPropertyName("event")] public string? Event { get; set; }
    public string? From { get; set; }
    public string? Message { get; set; }
    [JsonPropertyName("Message_Localised")] public string? MessageLocalised { get; set; }
    public string? Channel { get; set; }

    public override JournalEventType EventType => JournalEventType.ReceiveText;
}