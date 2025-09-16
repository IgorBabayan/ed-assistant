namespace EdAssistant.Models.Scan;

public class FSDJumpEvent : JournalEvent
{
    public override JournalEventType EventType => JournalEventType.FSDJump;
    
    [JsonPropertyName("StarSystem")]
    public string StarSystem { get; set; } = string.Empty;
    
    [JsonPropertyName("SystemAddress")]
    public long SystemAddress { get; set; }
    
    [JsonPropertyName("JumpDist")]
    public double JumpDist { get; set; }
    
    [JsonPropertyName("FuelUsed")]
    public double FuelUsed { get; set; }
    
    [JsonPropertyName("FuelLevel")]
    public double FuelLevel { get; set; }
}