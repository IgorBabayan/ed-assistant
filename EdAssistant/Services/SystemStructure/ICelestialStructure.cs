namespace EdAssistant.Services.SystemStructure;

public interface ICelestialStructure
{
    IReadOnlyList<Star> Stars { get; }
    SystemNode SystemRoot { get; }
    long SystemAddress { get; set; }
    string SystemName { get; set; }
    void AddScanEvent(ScanEvent scanEvent);
    void BuildHierarchy();
}