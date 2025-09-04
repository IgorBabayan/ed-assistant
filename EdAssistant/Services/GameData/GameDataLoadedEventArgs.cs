namespace EdAssistant.Services.GameData;

public class GameDataLoadedEventArgs(Type dataType, object data) : EventArgs
{
    public Type DataType { get; } = dataType;
    public object Data { get; } = data;
}