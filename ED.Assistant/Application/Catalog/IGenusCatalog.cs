namespace ED.Assistant.Application.Catalog;

/// <summary>
/// Read-only genus catalog (seed data). Loaded from the database once and shared.
/// The returned entities must not be modified.
/// </summary>
public interface IGenusCatalog
{
    /// <summary>All genera with their spawn rules.</summary>
    Task<IReadOnlyList<Genus>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Genera keyed by codex name (case-insensitive).</summary>
    // Catalog API kept alongside GetAllAsync; no caller in the app yet
    // ReSharper disable once UnusedMember.Global
    Task<IReadOnlyDictionary<string, Genus>> GetByCodexNameAsync(CancellationToken cancellationToken = default);
}