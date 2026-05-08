using ED.Assistant.Data.Storage;
using Microsoft.EntityFrameworkCore.Design;

namespace ED.Assistant.Data.Seed;

internal sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	public AppDbContext CreateDbContext(string[] args)
	{
		var dbPathProvider = new DbPathProvider();
		var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
		optionsBuilder.UseSqlite($"Data Source={dbPathProvider.GetDatabasePath()}");
		return new(optionsBuilder.Options);
	}
}