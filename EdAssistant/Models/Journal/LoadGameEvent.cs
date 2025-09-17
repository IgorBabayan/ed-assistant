namespace EdAssistant.Models.Journal;

public class LoadGameEvent : JournalEvent
{
    [JsonPropertyName("FID")]
    public required string FId { get; set; }

    [JsonPropertyName("Commander")]
    public required string Commander { get; set; }

    [JsonPropertyName("Horizons")]
    public bool Horizons { get; set; }

    [JsonPropertyName("Odyssey")]
    public bool Odyssey { get; set; }

    [JsonPropertyName("Ship")]
    public required string Ship { get; set; }

    [JsonPropertyName("Ship_Localised")]
    public required string ShipLocalised { get; set; }

    [JsonPropertyName("ShipID")]
    public int ShipId { get; set; }

    [JsonPropertyName("ShipName")]
    public required string ShipName { get; set; }

    [JsonPropertyName("ShipIdent")]
    public required string ShipIdent { get; set; }

    [JsonPropertyName("FuelLevel")]
    public double FuelLevel { get; set; }

    [JsonPropertyName("FuelCapacity")]
    public double FuelCapacity { get; set; }

    [JsonPropertyName("GameMode")]
    public required string GameMode { get; set; }

    [JsonPropertyName("Credits")]
    public long Credits { get; set; }

    [JsonPropertyName("Loan")]
    public long Loan { get; set; }

    [JsonPropertyName("language")]
    public required string Language { get; set; }

    [JsonPropertyName("gameversion")]
    public required string GameVersion { get; set; }

    [JsonPropertyName("build")]
    public required string Build { get; set; }

    public override JournalEventType EventType => JournalEventType.LoadGame;
}