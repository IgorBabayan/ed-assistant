namespace EdAssistant.Models.Journal;

public class ReceiveTextEvent : JournalEvent
{
    [JsonPropertyName("From")]
    public required string From { get; set; }

    [JsonPropertyName("From_Localised")]
    public required string FromLocalised { get; set; }

    [JsonPropertyName("Message")]
    public required string Message { get; set; }

    [JsonPropertyName("Message_Localised")]
    public required string MessageLocalised { get; set; }

    [JsonPropertyName("Channel")]
    public required string Channel { get; set; }

    public override JournalEventType EventType => JournalEventType.ReceiveText;
}