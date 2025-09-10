namespace EdAssistant.ViewModels;

public abstract class BaseViewModel : ObservableObject, IDisposable
{
    public virtual void Dispose() { }
}