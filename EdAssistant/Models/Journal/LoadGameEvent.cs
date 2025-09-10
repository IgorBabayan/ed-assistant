namespace EdAssistant.Models.Journal;

public class LoadGameEvent : JournalEvent
{
    [JsonPropertyName("FID")]
    public string FID { get; set; }

    [JsonPropertyName("Commander")]
    public string Commander { get; set; }

    [JsonPropertyName("Horizons")]
    public bool Horizons { get; set; }

    [JsonPropertyName("Odyssey")]
    public bool Odyssey { get; set; }

    [JsonPropertyName("Ship")]
    public string Ship { get; set; }

    [JsonPropertyName("Ship_Localised")]
    public string ShipLocalised { get; set; }

    [JsonPropertyName("ShipID")]
    public int ShipID { get; set; }

    [JsonPropertyName("ShipName")]
    public string ShipName { get; set; }

    [JsonPropertyName("ShipIdent")]
    public string ShipIdent { get; set; }

    [JsonPropertyName("FuelLevel")]
    public double FuelLevel { get; set; }

    [JsonPropertyName("FuelCapacity")]
    public double FuelCapacity { get; set; }

    [JsonPropertyName("GameMode")]
    public string GameMode { get; set; }

    [JsonPropertyName("Credits")]
    public long Credits { get; set; }

    [JsonPropertyName("Loan")]
    public long Loan { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; }

    [JsonPropertyName("gameversion")]
    public string GameVersion { get; set; }

    [JsonPropertyName("build")]
    public string Build { get; set; }

    public override JournalEventType EventType => JournalEventType.LoadGame;
}