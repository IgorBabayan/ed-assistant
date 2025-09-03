using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EdAssistant.Models.Cargo;

public class CargoEvent
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("event")]
    public string Event { get; set; }

    [JsonPropertyName("Vessel")]
    public string Vessel { get; set; }

    [JsonPropertyName("Count")]
    public int Count { get; set; }

    [JsonPropertyName("Inventory")]
    public List<InventoryItem> Inventory { get; set; } = new();
}