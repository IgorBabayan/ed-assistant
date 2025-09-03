using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EdAssistant.Models.ShipLocker;

public class ShipLockerEvent
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("event")]
    public string Event { get; set; }

    [JsonPropertyName("Items")]
    public List<InventoryItem> Items { get; set; } = new();

    [JsonPropertyName("Components")]
    public List<InventoryItem> Components { get; set; } = new();

    [JsonPropertyName("Consumables")]
    public List<InventoryItem> Consumables { get; set; } = new();

    [JsonPropertyName("Data")]
    public List<InventoryItem> Data { get; set; } = new();
}