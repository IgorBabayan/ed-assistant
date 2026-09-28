namespace ED.Assistant.Application.Linux;

public interface IDesktopService
{
    void BuildDesktopFile();
    Task SaveDesktopFileAsync(CancellationToken cancellationToken);
}