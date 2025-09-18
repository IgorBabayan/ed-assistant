namespace EdAssistant.ViewModels;

public abstract class BaseViewModel : ObservableObject, IDisposable
{
    private bool _disposed;

    public void Dispose()
    {
        if (_disposed)
            return;
        
        _disposed = true;
        OnDispose(true);
        
        GC.SuppressFinalize(this);
    }
    
    protected virtual void OnDispose(bool disposing) { }
}