namespace ED.Assistant.Domain.Types;

public sealed record BioAlert(
    long SystemAddress,
    int BodyId,
    string BodyName,
    int Count,
    string[]? Genera);