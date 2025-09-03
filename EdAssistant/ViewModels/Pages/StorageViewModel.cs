using System;
using EdAssistant.Helpers.Attributes;
using EdAssistant.Models.Enums;
using EdAssistant.Models.ShipLocker;
using EdAssistant.Services.GameData;

namespace EdAssistant.ViewModels.Pages;

[DockMapping(DockEnum.Storage)]
public sealed partial class StorageViewModel : PageViewModel
{
    private readonly IGameDataService _gameDataService;

    public StorageViewModel(IGameDataService gameDataService)
    {
        _gameDataService = gameDataService;
        _gameDataService.DataLoaded += OnGameDataLoaded;

        var existingData = _gameDataService.GetData<ShipLockerEvent>();
        if (existingData is not null)
        {
            ProcessShipLockerData(existingData);
        }
    }

    private void OnGameDataLoaded(object? sender, GameDataLoadedEventArgs e)
    {
        if (e.DataType == typeof(ShipLockerEvent) && e.Data is ShipLockerEvent shipLocker)
        {
            ProcessShipLockerData(shipLocker);
        }
    }

    private void ProcessShipLockerData(ShipLockerEvent data)
    {
        throw new NotImplementedException();
    }
}