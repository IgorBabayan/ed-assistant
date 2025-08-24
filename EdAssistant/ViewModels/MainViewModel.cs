using EdAssistant.Translations;

namespace EdAssistant.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public string WindowTitle => Localization.Instance["MainWindow.Title"];
}
