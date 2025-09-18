namespace EdAssistant.Models.Journal;

public class ShipLockerEvent : JournalEvent
{
    public override JournalEventTypeEnum EventTypeEnum => JournalEventTypeEnum.ShipLocker;
    
    [JsonPropertyName("Items")]
    public List<Item> Items { get; set; } = new();

    [JsonPropertyName("Components")]
    public List<Component> Components { get; set; } = new();

    [JsonPropertyName("Consumables")]
    public List<Consumable> Consumables { get; set; } = new();

    [JsonPropertyName("Data")]
    public List<DataItem> Data { get; set; } = new();
}