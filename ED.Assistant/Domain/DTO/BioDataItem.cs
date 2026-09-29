namespace ED.Assistant.Domain.DTO;

public sealed class BioDataItem
{
    [JsonPropertyName("Genus")]
    public string GenusId { get; set; } = string.Empty;

    [JsonPropertyName("Genus_Localised")]
    public string Genus { get; set; } = string.Empty;

    [JsonPropertyName("Species")]
    public string SpeciesId { get; set; } = string.Empty;

    [JsonPropertyName("Species_Localised")]
    public string Species { get; set; } = string.Empty;

    [JsonPropertyName("Variant")]
    public string VariantId { get; set; } = string.Empty;

    [JsonPropertyName("Variant_Localised")]
    public string Variant { get; set; } = string.Empty;

    [JsonPropertyName("Value")]
    public long Value { get; set; }

    [JsonPropertyName("Bonus")]
    public long Bonus { get; set; }
}