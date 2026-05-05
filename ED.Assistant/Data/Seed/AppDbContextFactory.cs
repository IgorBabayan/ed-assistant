using ED.Assistant.Data.Seed.Path;
using Microsoft.EntityFrameworkCore.Design;

namespace ED.Assistant.Data.Seed;

internal sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	private readonly IDbPathProvider _dbPathProvider;

	public AppDbContextFactory(IDbPathProvider dbPathProvider) => _dbPathProvider = dbPathProvider;

	public AppDbContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
		optionsBuilder.UseSqlite($"Data Source={_dbPathProvider.GetBioSamplesDbPath()}");
		return new(optionsBuilder.Options);
	}
}