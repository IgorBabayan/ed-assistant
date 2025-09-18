namespace EdAssistant.Models.Journal;

public class CargoEvent : JournalEvent
{
    [JsonPropertyName("Vessel")]
    public string Vessel { get; set; }

    [JsonPropertyName("Count")]
    public int Count { get; set; }

    [JsonPropertyName("Inventory")]
    public List<CargoItem> Inventory { get; set; } = new();

    public override JournalEventTypeEnum EventTypeEnum => JournalEventTypeEnum.Cargo;
}