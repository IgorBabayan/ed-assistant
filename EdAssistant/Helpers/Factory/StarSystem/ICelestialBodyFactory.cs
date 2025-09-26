namespace EdAssistant.Helpers.Factory.StarSystem;

public interface ICelestialBodyFactory
{
    CelestialBody? Create(JournalEvent scanEvent);
}