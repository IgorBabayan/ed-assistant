namespace ED.Assistant.Data.Seed;

public interface IBioDataSeeder
{
	Task SeedAsync(CancellationToken cancellationToken = default);
}
