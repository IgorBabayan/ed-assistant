namespace EdAssistant.Services.Color;

class ResourceService : IResourceService
{
    public IBrush GetBrush(string key)
    {
        return Application.Current!.FindResource(key) as IBrush ?? 
               new SolidColorBrush(Colors.Transparent);
    }

    public Avalonia.Media.Color GetColor(string key)
    {
        if (Application.Current!.FindResource(key) is Avalonia.Media.Color color)
            return color;
        
        if (Application.Current!.FindResource(key) is SolidColorBrush brush)
            return brush.Color;
            
        return Colors.Transparent;
    }
}