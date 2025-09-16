using Material = EdAssistant.Models.Scan.Material;

namespace EdAssistant.DTO.Scan;

public class Planet : CelestialBody
{
    public bool? TidalLock { get; set; }
    public string? TerraformState { get; set; }
    public string? PlanetClass { get; set; }
    public string? Atmosphere { get; set; }
    public string? AtmosphereType { get; set; }
    public List<AtmosphereComposition>? AtmosphereComposition { get; set; }
    public string? Volcanism { get; set; }
    public double? MassEM { get; set; }
    public double? SurfaceGravity { get; set; }
    public double? SurfaceTemperature { get; set; }
    public double? SurfacePressure { get; set; }
    public bool? Landable { get; set; }
    public List<Material>? Materials { get; set; }
    public Composition? Composition { get; set; }
    public string? ReserveLevel { get; set; }
    public List<Ring>? Rings { get; set; }

    public override string TypeInfo => PlanetClass ?? "Planet";
    public override string LandableInfo => Landable == true ? "Yes" : "No";
    public override string MassInfo => MassEM?.ToString("N3") + " M⊕" ?? "";
}