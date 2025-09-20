namespace EdAssistant.Services.Desktop;

public interface IDesktopService
{
    Task CreateDesktopFile();
    Task Save();
}