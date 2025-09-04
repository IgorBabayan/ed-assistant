namespace EdAssistant.Services.Navigate;

public interface INavigationService
{
    void NavigateTo(DockEnum dock);
    PageViewModel? Current { get; }
    event EventHandler? CurrentChanged;
}