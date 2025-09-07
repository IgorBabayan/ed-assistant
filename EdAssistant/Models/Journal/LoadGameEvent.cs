namespace EdAssistant.Models.Journal;

public sealed class LoadGameEvent : JournalEventBase
{
    [JsonPropertyName("event")] public string? Event { get; set; }
    public string? FID { get; set; }
    public string? Commander { get; set; }
    public bool? Horizons { get; set; }
    public bool? Odyssey { get; set; }
    public string? Ship { get; set; }
    [JsonPropertyName("Ship_Localised")] public string? ShipLocalised { get; set; }
    public int ShipID { get; set; }
    public string? ShipName { get; set; }
    public string? ShipIdent { get; set; }
    public double FuelLevel { get; set; }
    public double FuelCapacity { get; set; }
    public string? GameMode { get; set; }
    public long Credits { get; set; }
    public long Loan { get; set; }
    public string? language { get; set; }
    public string? gameversion { get; set; }
    public string? build { get; set; }

    public override JournalEventType EventType => JournalEventType.LoadGame;
}