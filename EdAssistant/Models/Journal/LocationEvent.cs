namespace EdAssistant.Models.Journal;

public class LocationEvent : JournalEvent
{
    [JsonPropertyName("DistFromStarLS")]
    public double DistFromStarLS { get; set; }

    [JsonPropertyName("Docked")]
    public bool Docked { get; set; }

    [JsonPropertyName("StationName")]
    public string? StationName { get; set; }

    [JsonPropertyName("StationType")]
    public required string StationType { get; set; }

    [JsonPropertyName("MarketID")]
    public long MarketId { get; set; }

    [JsonPropertyName("StarSystem")]
    public required string StarSystem { get; set; }

    [JsonPropertyName("SystemAddress")]
    public long SystemAddress { get; set; }

    [JsonPropertyName("StarPos")]
    public List<double> StarPos { get; set; } = new();

    [JsonPropertyName("SystemAllegiance")]
    public required string SystemAllegiance { get; set; }

    [JsonPropertyName("SystemEconomy")]
    public required string SystemEconomy { get; set; }

    [JsonPropertyName("SystemEconomy_Localised")]
    public required string SystemEconomyLocalised { get; set; }

    [JsonPropertyName("SystemGovernment")]
    public required string SystemGovernment { get; set; }

    [JsonPropertyName("SystemGovernment_Localised")]
    public required string SystemGovernmentLocalised { get; set; }

    [JsonPropertyName("SystemSecurity")]
    public required string SystemSecurity { get; set; }

    [JsonPropertyName("SystemSecurity_Localised")]
    public required string SystemSecurityLocalised { get; set; }

    [JsonPropertyName("Population")]
    public long Population { get; set; }

    [JsonPropertyName("Body")]
    public required string Body { get; set; }

    [JsonPropertyName("BodyId")]
    public int BodyId { get; set; }

    [JsonPropertyName("BodyType")]
    public required string BodyType { get; set; }

    public override JournalEventType EventType => JournalEventType.Location;
}