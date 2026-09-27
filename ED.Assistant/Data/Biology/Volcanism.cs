namespace ED.Assistant.Data.Biology;

enum VolcanismEnum
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
    public string Name { get; set; }
    
    public ICollection<Rule> Rules { get; set; } = [];
}