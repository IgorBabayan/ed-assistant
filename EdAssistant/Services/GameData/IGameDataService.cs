using System;
using System.Threading.Tasks;

namespace EdAssistant.Services.GameData;

public interface IGameDataService
{
    Task<object?> LoadDataAsync(Type type, string filePath);
    Task LoadAllGameDataAsync(string journalsFolder);
    T? GetData<T>() where T : class;
    object? GetData(Type type);
    event EventHandler<GameDataLoadedEventArgs> DataLoaded;
}