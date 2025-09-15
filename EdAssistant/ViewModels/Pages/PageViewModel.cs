namespace EdAssistant.ViewModels.Pages;

public abstract partial class PageViewModel : BaseViewModel { }

[DockMapping(DockEnum.MarketConnector)]
public sealed partial class MarketConnectorViewModel : PageViewModel { }
[DockMapping(DockEnum.Log)]
public sealed partial class LogViewModel : PageViewModel { }