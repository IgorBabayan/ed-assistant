namespace EdAssistant.ViewModels.Pages;

[DockMapping(DockEnum.Cargo)]
public sealed partial class CargoViewModel : PageViewModel
{
    private readonly IGameDataService _gameDataService;

    public CargoViewModel(IGameDataService gameDataService)
    {
        _gameDataService = gameDataService;
        _gameDataService.DataLoaded += OnGameDataLoaded;

        var existingData = _gameDataService.GetData<CargoEvent>();
        if (existingData is not null)
        {
            ProcessCargoData(existingData);
        }
    }

    private void OnGameDataLoaded(object? sender, GameDataLoadedEventArgs e)
    {
        if (e.DataType == typeof(CargoEvent) && e.Data is CargoEvent cargoData)
        {
            ProcessCargoData(cargoData);
        }
    }

    private void ProcessCargoData(CargoEvent cargoData)
    {
        
    }
}