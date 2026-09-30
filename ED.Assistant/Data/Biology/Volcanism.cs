namespace ED.Assistant.Data.Biology;

internal enum VolcanismEnum
{
    None = 1,
    Any,
    Metallic,
    Silicate,
    Rocky,
    Water,
    CarbonDioxideGeysers,
    CarbonDioxide,
    Methane,
    Ammonia,
    Nitrogen,
    Carbon,
    MethaneMagma,
    MajorSilicate,
    MajorRocky,
    MajorMetallic,
    Geysers,
    RockyMagma,
    MajorRockyMagma,
    MajorSilicateVapour,
    MajorMetallicMagma,
    MetallicMagmaVolcanism,
    RockyMagmaVolcanism
}

public sealed class Volcanism
{
    public int Id { get; set; }
    // ReSharper disable once EntityFramework.ModelValidation.UnlimitedStringLength
    public required string Name { get; set; }
    
    public ICollection<Rule> Rules { get; set; } = [];
}