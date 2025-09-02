using CommunityToolkit.Mvvm.ComponentModel;
using EdAssistant.Helpers.Attributes;
using EdAssistant.Models.Enums;

namespace EdAssistant.ViewModels.Pages;

public abstract partial class PageViewModel : ObservableObject { }

[DockMapping(DockEnum.Home)]
public sealed partial class HomeViewModel : PageViewModel { }
[DockMapping(DockEnum.Materials)]
public sealed partial class MaterialsViewModel : PageViewModel { }
[DockMapping(DockEnum.Storage)]
public sealed partial class StorageViewModel : PageViewModel { }
[DockMapping(DockEnum.System)]
public sealed partial class SystemViewModel : PageViewModel { }
[DockMapping(DockEnum.Planet)]
public sealed partial class PlanetViewModel : PageViewModel { }
[DockMapping(DockEnum.MarketConnector)]
public sealed partial class MarketConnectorViewModel : PageViewModel { }
[DockMapping(DockEnum.Log)]
public sealed partial class LogViewModel : PageViewModel { }