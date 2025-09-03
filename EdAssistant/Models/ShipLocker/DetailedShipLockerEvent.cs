using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EdAssistant.Models.ShipLocker;

public class DetailedShipLockerEvent
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("event")]
    public string Event { get; set; }

    [JsonPropertyName("Items")]
    public List<Item> Items { get; set; } = new();

    [JsonPropertyName("Components")]
    public List<Component> Components { get; set; } = new();

    [JsonPropertyName("Consumables")]
    public List<Consumable> Consumables { get; set; } = new();

    [JsonPropertyName("Data")]
    public List<DataItem> Data { get; set; } = new();
}