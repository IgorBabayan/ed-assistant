namespace EdAssistant.ViewModels.Pages;

public abstract partial class PageViewModel : BaseViewModel { }

[DockMapping(DockEnum.System)]
public sealed partial class SystemViewModel : PageViewModel { }
[DockMapping(DockEnum.Planet)]
public sealed partial class PlanetViewModel : PageViewModel { }
[DockMapping(DockEnum.MarketConnector)]
public sealed partial class MarketConnectorViewModel : PageViewModel { }
[DockMapping(DockEnum.Log)]
public sealed partial class LogViewModel : PageViewModel { }