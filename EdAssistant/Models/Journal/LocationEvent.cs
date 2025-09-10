namespace EdAssistant.Models.Journal;

public class LocationEvent : JournalEvent
{
    [JsonPropertyName("DistFromStarLS")]
    public double DistFromStarLS { get; set; }

    [JsonPropertyName("Docked")]
    public bool Docked { get; set; }

    [JsonPropertyName("StationName")]
    public string StationName { get; set; }

    [JsonPropertyName("StationType")]
    public string StationType { get; set; }

    [JsonPropertyName("MarketID")]
    public long MarketID { get; set; }

    [JsonPropertyName("StarSystem")]
    public string StarSystem { get; set; }

    [JsonPropertyName("SystemAddress")]
    public long SystemAddress { get; set; }

    [JsonPropertyName("StarPos")]
    public List<double> StarPos { get; set; } = new();

    [JsonPropertyName("SystemAllegiance")]
    public string SystemAllegiance { get; set; }

    [JsonPropertyName("SystemEconomy")]
    public string SystemEconomy { get; set; }

    [JsonPropertyName("SystemEconomy_Localised")]
    public string SystemEconomyLocalised { get; set; }

    [JsonPropertyName("SystemGovernment")]
    public string SystemGovernment { get; set; }

    [JsonPropertyName("SystemGovernment_Localised")]
    public string SystemGovernmentLocalised { get; set; }

    [JsonPropertyName("SystemSecurity")]
    public string SystemSecurity { get; set; }

    [JsonPropertyName("SystemSecurity_Localised")]
    public string SystemSecurityLocalised { get; set; }

    [JsonPropertyName("Population")]
    public long Population { get; set; }

    [JsonPropertyName("Body")]
    public string Body { get; set; }

    [JsonPropertyName("BodyID")]
    public int BodyID { get; set; }

    [JsonPropertyName("BodyType")]
    public string BodyType { get; set; }

    public override JournalEventType EventType => JournalEventType.Location;
}