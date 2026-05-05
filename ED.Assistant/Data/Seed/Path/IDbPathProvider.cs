namespace ED.Assistant.Data.Seed.Path;

public interface IDbPathProvider
{
	string GetBioSamplesDbPath();
	string GetSeedPath();
	string GetSeedDataBasePath();
	bool BioSamplesDbExists();
}
