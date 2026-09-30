// ReSharper disable EntityFramework.ModelValidation.UnlimitedStringLength
namespace ED.Assistant.Data.Biology;

internal enum AtmosphereEnum
{
    None = 1,
    CarbonDioxide,
    Ammonia,
    Helium,
    Argon,
    Methane,
    Neon,
    NeonRich,
    Nitrogen,
    Oxygen,
    ArgonRich,
    CarbonDioxideRich,
    SulphurDioxide,
    Water,
    WaterRich,
    MethaneRich
}

public sealed class Atmosphere
{
    public int Id { get; set; }
    public required string Name { get; set; }
    
    public ICollection<Rule> Rules { get; set; } = [];
}