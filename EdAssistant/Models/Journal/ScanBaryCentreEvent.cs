namespace EdAssistant.Models.Journal;

public class ScanBaryCentreEvent : JournalEvent
{
    public override JournalEventTypeEnum EventTypeEnum => JournalEventTypeEnum.ScanBaryCentre;
    
    [JsonPropertyName("StarSystem")]
    public string StarSystem { get; set; }
    
    [JsonPropertyName("SystemAddress")]
    public long SystemAddress { get; set; }
    
    [JsonPropertyName("BodyID")]
    public int BodyId { get; set; }
    
    [JsonPropertyName("SemiMajorAxis")]
    public double? SemiMajorAxis { get; set; }
    
    [JsonPropertyName("Eccentricity")]
    public double? Eccentricity { get; set; }
    
    [JsonPropertyName("OrbitalInclination")]
    public double? OrbitalInclination { get; set; }
    
    [JsonPropertyName("Periapsis")]
    public double? Periapsis { get; set; }
    
    [JsonPropertyName("OrbitalPeriod")]
    public double? OrbitalPeriod { get; set; }
    
    [JsonPropertyName("AscendingNode")]
    public double? AscendingNode { get; set; }
    
    [JsonPropertyName("MeanAnomaly")]
    public double? MeanAnomaly { get; set; }
}