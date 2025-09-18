namespace EdAssistant.ViewModels;

public interface IAsyncInitializable
{
    Task InitializeAsync();
    bool IsInitialized { get; }
    bool IsInitializing { get; }
}