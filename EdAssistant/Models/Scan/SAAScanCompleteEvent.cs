namespace EdAssistant.Models.Scan;

public class SAAScanCompleteEvent : JournalEvent
{
    public override JournalEventType EventType => JournalEventType.SAAScanComplete;
    
    [JsonPropertyName("ProbesUsed")]
    public int ProbesUsed { get; set; }
    
    [JsonPropertyName("EfficiencyTarget")] 
    public int EfficiencyTarget { get; set; }
    
    [JsonPropertyName("SystemAddress")]
    public long SystemAddress { get; set; }
    
    [JsonPropertyName("BodyID")] 
    public int BodyId { get; set; }
    
    [JsonPropertyName("BodyName")]
    public required string BodyName { get; set; }
}