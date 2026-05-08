namespace ED.Assistant.Data.Storage;

public interface IDbPathProvider
{
	string GetDatabasePath();
	string GetDatabaseDirectory();
}
