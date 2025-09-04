namespace EdAssistant.Models.ShipLocker;

public class InventoryItem
{
    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [JsonPropertyName("Name_Localised")]
    public string NameLocalised { get; set; }

    [JsonPropertyName("OwnerID")]
    public int OwnerID { get; set; }

    [JsonPropertyName("MissionID")]
    public long? MissionID { get; set; }

    [JsonPropertyName("Count")]
    public int Count { get; set; }
}

public class Item : InventoryItem { }
public class Component : InventoryItem { }
public class Consumable : InventoryItem { }
public class DataItem : InventoryItem { }

public class CategorizedInventoryItem : InventoryItem
{
    public ItemCategory Category { get; set; }
}