using ED.Assistant.Helpers;
using Microsoft.EntityFrameworkCore.Design;

namespace ED.Assistant.Data.Seed;

internal sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	public AppDbContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
		var dbPath = DbPathProvider.GetDatabasePath();

		optionsBuilder.UseSqlite($"Data Source={dbPath}");
		return new(optionsBuilder.Options);
	}
}