namespace EdAssistant.Models.Journal;

public class LocationEvent : JournalEvent
{
    public override JournalEventTypeEnum EventTypeEnum => JournalEventTypeEnum.Location;

    [JsonPropertyName("DistFromStarLS")]
    public double DistFromStarLS { get; set; }

    [JsonPropertyName("Docked")]
    public bool Docked { get; set; }

    [JsonPropertyName("Taxi")]
    public bool Taxi { get; set; }

    [JsonPropertyName("Multicrew")]
    public bool Multicrew { get; set; }

    [JsonPropertyName("StarSystem")]
    public string StarSystem { get; set; } = string.Empty;

    [JsonPropertyName("SystemAddress")]
    public long SystemAddress { get; set; }

    [JsonPropertyName("StarPos")]
    public double[] StarPos { get; set; } = new double[3];

    [JsonPropertyName("SystemAllegiance")]
    public string? SystemAllegiance { get; set; }

    [JsonPropertyName("SystemEconomy")]
    public string? SystemEconomy { get; set; }

    [JsonPropertyName("SystemEconomy_Localised")]
    public string? SystemEconomyLocalised { get; set; }

    [JsonPropertyName("SystemSecondEconomy")]
    public string? SystemSecondEconomy { get; set; }

    [JsonPropertyName("SystemSecondEconomy_Localised")]
    public string? SystemSecondEconomyLocalised { get; set; }

    [JsonPropertyName("SystemGovernment")]
    public string? SystemGovernment { get; set; }

    [JsonPropertyName("SystemGovernment_Localised")]
    public string? SystemGovernmentLocalised { get; set; }

    [JsonPropertyName("SystemSecurity")]
    public string? SystemSecurity { get; set; }

    [JsonPropertyName("SystemSecurity_Localised")]
    public string? SystemSecurityLocalised { get; set; }

    [JsonPropertyName("Population")]
    public long? Population { get; set; }

    [JsonPropertyName("Body")]
    public string? Body { get; set; }

    [JsonPropertyName("BodyID")]
    public int? BodyId { get; set; }

    [JsonPropertyName("BodyType")]
    public string? BodyType { get; set; }

    [JsonPropertyName("ControllingPower")]
    public string? ControllingPower { get; set; }

    [JsonPropertyName("Powers")]
    public List<string>? Powers { get; set; }

    [JsonPropertyName("PowerplayState")]
    public string? PowerplayState { get; set; }

    [JsonPropertyName("PowerplayStateControlProgress")]
    public double? PowerplayStateControlProgress { get; set; }

    [JsonPropertyName("PowerplayStateReinforcement")]
    public int? PowerplayStateReinforcement { get; set; }

    [JsonPropertyName("PowerplayStateUndermining")]
    public int? PowerplayStateUndermining { get; set; }

    [JsonPropertyName("Factions")]
    public List<Faction>? Factions { get; set; }

    [JsonPropertyName("SystemFaction")]
    public SystemFaction? SystemFaction { get; set; }
}