namespace EdAssistant.Services.Color;

public interface IResourceService
{
    IBrush GetBrush(string key);
    Avalonia.Media.Color GetColor(string key);
}