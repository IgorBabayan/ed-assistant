namespace EdAssistant.Models.Scan;

public class FSDJumpEvent : JournalEvent
{
    public override JournalEventType EventType => JournalEventType.FSDJump;
    public string StarSystem { get; set; } = string.Empty;
    public long SystemAddress { get; set; }
    public double JumpDist { get; set; }
    public double FuelUsed { get; set; }
    public double FuelLevel { get; set; }
}