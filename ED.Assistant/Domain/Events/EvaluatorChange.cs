namespace ED.Assistant.Domain.Events;

public abstract record EvaluatorChange(DateTime Timestamp);

public sealed record OrganicSampled(DateTime Timestamp, string SpeciesId, bool HasFirstFootStep)
    : EvaluatorChange(Timestamp);

public sealed record OrganicDataSold(DateTime Timestamp, IReadOnlyList<string> SpeciesIds)
    : EvaluatorChange(Timestamp);