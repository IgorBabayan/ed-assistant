using CommunityToolkit.Mvvm.ComponentModel;

namespace ED.Assistant.Plugins.MarketConnector.Settings;

public sealed partial class EddnSettingsViewModel(IPluginSettings<EddnSettings> settings)
    : ObservableObject, IPluginSettingsViewModel
{
    [ObservableProperty] public partial bool SendStationData { get; set; }
    [ObservableProperty] public partial bool SendSystemData { get; set; }
    [ObservableProperty] public partial bool DelayUntilDocked { get; set; }

    public Task LoadAsync(CancellationToken cancellationToken)
    {
        var current = settings.Current;
        SendStationData = current.SendStationData;
        SendSystemData = current.SendSystemData;
        DelayUntilDocked = current.DelayUntilDocked;
        return Task.CompletedTask;
    }

    // "with" keeps fields this page doesn't edit
    public Task SaveAsync(CancellationToken cancellationToken) =>
        settings.SaveAsync(settings.Current with
        {
            SendStationData = SendStationData,
            SendSystemData = SendSystemData,
            DelayUntilDocked = DelayUntilDocked
        }, cancellationToken);
}

public sealed partial class EdsmSettingsViewModel(IPluginSettings<EdsmSettings> settings)
    : ObservableObject, IPluginSettingsViewModel
{
    [ObservableProperty] public partial bool Enabled { get; set; }
    [ObservableProperty] public partial string? CommanderName { get; set; }
    [ObservableProperty] public partial string? ApiKey { get; set; }

    public Task LoadAsync(CancellationToken cancellationToken)
    {
        var current = settings.Current;
        Enabled = current.Enabled;
        CommanderName = current.CommanderName;
        ApiKey = current.ApiKey;
        return Task.CompletedTask;
    }

    public string? Validate() => Enabled switch
    {
        true when string.IsNullOrWhiteSpace(CommanderName) => "Commander name is required",
        true when string.IsNullOrWhiteSpace(ApiKey) => "API key is required",
        _ => null
    };

    public Task SaveAsync(CancellationToken cancellationToken) =>
        settings.SaveAsync(settings.Current with
        {
            Enabled = Enabled,
            CommanderName = CommanderName?.Trim(),
            ApiKey = ApiKey?.Trim()
        }, cancellationToken);
}
