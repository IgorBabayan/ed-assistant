using CommunityToolkit.Mvvm.ComponentModel;

namespace EdAssistant.ViewModels.Pages;

public abstract partial class PageViewModel : ObservableObject { }

public sealed partial class HomeViewModel : PageViewModel { }
public sealed partial class MaterialsViewModel : PageViewModel { }
public sealed partial class StorageViewModel : PageViewModel { }
public sealed partial class SystemViewModel : PageViewModel { }
public sealed partial class PlanetViewModel : PageViewModel { }
public sealed partial class MarketConnectorViewModel : PageViewModel { }
public sealed partial class LogViewModel : PageViewModel { }