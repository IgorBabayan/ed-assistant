namespace ED.Assistant.Domain.Events;

public sealed class LocationEvent : BaseJournalEvent
{
    internal const string EventName = "Location";

    [JsonPropertyName("SystemAddress")]
    public long SystemAddress { get; set; }

    [JsonPropertyName("StarSystem")]
    public string StarSystem { get; set; } = string.Empty;
}