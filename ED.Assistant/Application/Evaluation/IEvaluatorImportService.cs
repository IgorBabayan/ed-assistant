namespace ED.Assistant.Application.Evaluation;

public interface IEvaluatorImportService
{
    /// <summary>Raised when an import finishes, successfully or not.</summary>
    event EventHandler<EvaluatorImportResult>? Imported;

    Task<EvaluatorImportResult> ImportAsync(string logFolder, CancellationToken cancellationToken = default);
}

public sealed record EvaluatorImportResult(string Folder, int Added, string? Error = null)
{
    public bool IsSuccess => Error is null;
}