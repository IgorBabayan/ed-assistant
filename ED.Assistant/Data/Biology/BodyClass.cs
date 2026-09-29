namespace ED.Assistant.Data.Biology;

enum BodyClassEnum
{
    RockyBody = 1,
    HighMetalContentBody,
    IcyBody,
    RockyIceBody,
    MetalRichBody,
    EarthLikeBody,
    GasGiantWithWaterBasedLife,
    WaterGiant,
    AmmoniaWorld,
    WaterWorld,
    GasGiantWithAmmoniaBasedLife
}

public sealed class BodyClass
{
    public int Id { get; set; }
    public required string Name { get; set; }
    
    public ICollection<Rule> Rules { get; set; } = [];
}