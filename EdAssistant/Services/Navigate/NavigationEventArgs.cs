namespace EdAssistant.Services.Navigate;

public class NavigationEventArgs : EventArgs
{
    public required Type ViewModelType { get; set; }
    public object? ViewModel { get; set; }
    public object? Parameter { get; set; }
    public bool Cancel { get; set; }
}