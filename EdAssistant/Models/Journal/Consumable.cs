namespace EdAssistant.Models.Journal;

public class Consumable
{
    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [JsonPropertyName("Name_Localised")]
    public string NameLocalised { get; set; }

    [JsonPropertyName("OwnerID")]
    public int OwnerId { get; set; }

    [JsonPropertyName("Count")]
    public int Count { get; set; }
}