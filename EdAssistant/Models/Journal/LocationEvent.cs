namespace EdAssistant.Models.Journal;

public sealed class LocationEvent : JournalEventBase
{
    public sealed class FactionStateEntry
    {
        public string? State { get; set; }
        public int? Trend { get; set; }
    }

    public sealed class FactionEntry
    {
        public string? Name { get; set; }
        public string? FactionState { get; set; }
        public string? Government { get; set; }
        public double Influence { get; set; }
        public string? Allegiance { get; set; }
        public string? Happiness { get; set; }
        [JsonPropertyName("Happiness_Localised")] public string? HappinessLocalised { get; set; }
        public double? MyReputation { get; set; }

        public List<FactionStateEntry>? ActiveStates { get; set; }
        public List<FactionStateEntry>? PendingStates { get; set; }
        public List<FactionStateEntry>? RecoveringStates { get; set; }
    }

    public sealed class ConflictSide
    {
        public string? Name { get; set; }
        public string? Stake { get; set; }
        public int WonDays { get; set; }
    }

    public sealed class Conflict
    {
        public string? WarType { get; set; }
        public string? Status { get; set; }
        public ConflictSide? Faction1 { get; set; }
        public ConflictSide? Faction2 { get; set; }
    }

    [JsonPropertyName("event")] public string? Event { get; set; }

    public double? DistFromStarLS { get; set; }
    public bool Docked { get; set; }
    public string? StationName { get; set; }
    public string? StationType { get; set; }
    public long? MarketID { get; set; }
    public FactionRef? StationFaction { get; set; }
    public string? StationGovernment { get; set; }
    [JsonPropertyName("StationGovernment_Localised")] public string? StationGovernmentLocalised { get; set; }
    public List<string>? StationServices { get; set; }
    public string? StationEconomy { get; set; }
    [JsonPropertyName("StationEconomy_Localised")] public string? StationEconomyLocalised { get; set; }
    public List<EconomySlice>? StationEconomies { get; set; }

    public bool? Taxi { get; set; }
    public bool? Multicrew { get; set; }
    public string? StarSystem { get; set; }
    public long? SystemAddress { get; set; }
    public List<double>? StarPos { get; set; }
    public string? SystemAllegiance { get; set; }
    public string? SystemEconomy { get; set; }
    [JsonPropertyName("SystemEconomy_Localised")] public string? SystemEconomyLocalised { get; set; }
    public string? SystemSecondEconomy { get; set; }
    [JsonPropertyName("SystemSecondEconomy_Localised")] public string? SystemSecondEconomyLocalised { get; set; }
    public string? SystemGovernment { get; set; }
    [JsonPropertyName("SystemGovernment_Localised")] public string? SystemGovernmentLocalised { get; set; }
    public string? SystemSecurity { get; set; }
    [JsonPropertyName("SystemSecurity_Localised")] public string? SystemSecurityLocalised { get; set; }
    public long? Population { get; set; }
    public string? Body { get; set; }
    public int? BodyID { get; set; }
    public string? BodyType { get; set; }

    public string? ControllingPower { get; set; }
    public List<string>? Powers { get; set; }
    public string? PowerplayState { get; set; }
    public double? PowerplayStateControlProgress { get; set; }
    public int? PowerplayStateReinforcement { get; set; }
    public int? PowerplayStateUndermining { get; set; }

    public List<FactionEntry>? Factions { get; set; }
    public FactionRef? SystemFaction { get; set; }
    public List<Conflict>? Conflicts { get; set; }

    public override JournalEventType EventType => JournalEventType.Location;
}