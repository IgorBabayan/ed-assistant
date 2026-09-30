using ED.Assistant.Data.Storage;
using Microsoft.EntityFrameworkCore.Design;

namespace ED.Assistant.Data.Seed;

internal sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
	
	public AppDbContext CreateDbContext(string[] args)
	{
		var dbPathProvider = new DbPathProvider();
		var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
		optionsBuilder.UseSqlite(new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder
                { DataSource = dbPathProvider.GetDatabasePath() }.ToString());
		return new(optionsBuilder.Options);
	}
}