namespace ED.Assistant.Application.Linux;

internal class NullDesktopService : IDesktopService
{
    public void BuildDesktopFile() => throw new NotImplementedException();
    public Task SaveDesktopFileAsync(CancellationToken cancellationToken) => throw new NotImplementedException();
}