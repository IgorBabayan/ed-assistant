using ED.Assistant.Data;
using ED.Assistant.Data.Biology;
using ED.Assistant.Data.Seed;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ED.Assistant.Tests.Data;

[TestClass]
public sealed class BioCatalogTests
{
	private static AppDbContext CreateContext(SqliteConnection connection) =>
		new(new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options);

	[TestMethod]
	public async Task FreshDatabaseImportsEveryRuleAndUsesBodyTypeForeignKeys()
	{
		await using var connection = new SqliteConnection("Data Source=:memory:");
		await connection.OpenAsync();
		await using var db = CreateContext(connection);
		await new BioDataSeeder(db).SeedAsync();
		db.ChangeTracker.Clear();

		Assert.AreEqual(115, await db.BioSpecies.CountAsync());
		Assert.AreEqual(19, await db.BioGenera.CountAsync());
		Assert.AreEqual(254, await db.BioSpawnRules.CountAsync());
		Assert.AreEqual(11, await db.BodyTypes.CountAsync());
		Assert.AreEqual(19, await db.BioSpawnRules.Select(x => x.SourceFile).Distinct().CountAsync());
		Assert.IsFalse(await db.BioSpawnRuleBodyTypes.AnyAsync(x => x.BodyTypeId <= 0));

		var arcus = await db.BioSpecies.Include(x => x.SpawnRules).ThenInclude(x => x.BodyTypes)
			.ThenInclude(x => x.BodyType).SingleAsync(x => x.Name == "Aleoida Arcus");
		var rule = arcus.SpawnRules.Single();
		Assert.AreEqual(175d, rule.MinTemperatureK);
		Assert.AreEqual(180d, rule.MaxTemperatureK);
		Assert.AreEqual(0.04d, rule.MinGravityG);
		Assert.AreEqual(0.276d, rule.MaxGravityG);
		Assert.AreEqual(0.0161d, rule.MinPressureAtmospheres);
		Assert.IsNull(rule.MaxPressureAtmospheres);
		CollectionAssert.AreEquivalent(new[] { "Rocky body", "High metal content body" },
			rule.BodyTypes.Select(x => x.BodyType.Name).ToArray());
		Assert.AreEqual(VolcanismMode.None, rule.VolcanismMode);
	}

	[TestMethod]
	public async Task ImportPreservesAlternativeRulesAndSpecialConditions()
	{
		await using var connection = new SqliteConnection("Data Source=:memory:");
		await connection.OpenAsync();
		await using var db = CreateContext(connection);
		await new BioDataSeeder(db).SeedAsync();
		db.ChangeTracker.Clear();
		var pluma = await db.BioSpecies.Include(x => x.SpawnRules).ThenInclude(x => x.Atmospheres)
			.ThenInclude(x => x.Atmosphere).SingleAsync(x => x.Name == "Electricae Pluma");
		Assert.AreEqual(2, pluma.SpawnRules.Count);
		var argon = pluma.SpawnRules.Single(x => x.SourceIndex == 1);
		var neon = pluma.SpawnRules.Single(x => x.SourceIndex == 2);
		CollectionAssert.AreEquivalent(new[] { "Argon", "ArgonRich" },
			argon.Atmospheres.Select(x => x.Atmosphere.Name).ToArray());
		CollectionAssert.AreEquivalent(new[] { "Neon", "NeonRich" },
			neon.Atmospheres.Select(x => x.Atmosphere.Name).ToArray());
		Assert.AreEqual(0.26d, neon.MinGravityG);
		Assert.AreEqual(0.005d, neon.MaxPressureAtmospheres);
		Assert.IsTrue(neon.MaxPressureExclusive);
		Assert.IsTrue(await db.Set<BioSpawnRuleStar>().AnyAsync(x => x.Luminosity != null));
		Assert.IsTrue(await db.Set<BioSpawnRuleStar>().AnyAsync(x => x.Scope == StarScope.Parent));
		Assert.IsTrue(await db.Set<BioSpawnRuleVolcanism>().AnyAsync(x => x.Match == VolcanismMatch.Exact));
		Assert.IsTrue(await db.Set<BioSpawnRuleAtmosphereComponent>().AnyAsync(x => x.MinPercent == 1.05));
		Assert.IsTrue(await db.BioSpawnRules.AnyAsync(x => x.MaxOrbitalPeriodSeconds == 86400 && x.MaxOrbitalPeriodExclusive));
		Assert.IsTrue(await db.BioSpawnRules.AnyAsync(x => x.MinArrivalDistanceLs == 12000));
	}

	[TestMethod]
	public async Task RepeatedStartupDoesNotDuplicateOrReplaceImportedRows()
	{
		await using var connection = new SqliteConnection("Data Source=:memory:");
		await connection.OpenAsync();
		await using (var db = CreateContext(connection))
			await new BioDataSeeder(db).SeedAsync();
		await using var second = CreateContext(connection);
		var before = await second.BioSpawnRules.OrderBy(x => x.Id).Select(x => x.Id).ToArrayAsync();
		await new BioDataSeeder(second).SeedAsync();
		CollectionAssert.AreEqual(before, await second.BioSpawnRules.OrderBy(x => x.Id).Select(x => x.Id).ToArrayAsync());
		Assert.AreEqual(115, await second.BioSpecies.CountAsync());
		Assert.AreEqual(1, await second.BioCatalogVersions.CountAsync());
	}


	[TestMethod]
	public async Task MatchingCatalogHashRepairsMissingImportedRules()
	{
		await using var connection = new SqliteConnection("Data Source=:memory:");
		await connection.OpenAsync();

		await using (var first = CreateContext(connection))
		{
			await new BioDataSeeder(first).SeedAsync();
			await first.Database.ExecuteSqlRawAsync(
				"DELETE FROM BioSpawnRules WHERE SourceFile IS NOT NULL;");
			Assert.AreEqual(0,
				await first.BioSpawnRules.CountAsync(x => x.SourceFile != null));
			Assert.AreEqual(1, await first.BioCatalogVersions.CountAsync());
		}

		await using var repaired = CreateContext(connection);
		await new BioDataSeeder(repaired).SeedAsync();

		Assert.AreEqual(254,
			await repaired.BioSpawnRules.CountAsync(x => x.SourceFile != null));
		Assert.AreEqual(1, await repaired.BioCatalogVersions.CountAsync());
	}

	[TestMethod]
	public async Task UpgradePreservesInstalledIdsAndUnrelatedSpecies()
	{
		await using var connection = new SqliteConnection("Data Source=:memory:");
		await connection.OpenAsync();
		await using (var old = CreateContext(connection))
		{
			await old.GetService<IMigrator>().MigrateAsync("20260508074356_MoveSpawnRuleBodyTypesToRelation");
			await old.Database.ExecuteSqlRawAsync("""
				INSERT INTO VariantDeterminants (Id, Name) VALUES (901, 'Legacy determinant');
				INSERT INTO BioGenera (Id, Name, DisplayName) VALUES (801, 'Aleoida', 'Aleoida');
				INSERT INTO BioSpecies (Id, GenusId, Name, DisplayName, BaseValue, MinScanDistanceM, VariantDeterminantId)
				VALUES (701, 801, 'Aleoida Arcus', 'My Arcus', 10, 155, 901),
				       (702, 801, 'Custom organism', 'Custom organism', 10, 100, 901);
				INSERT INTO BioSpawnRules (Id, SpeciesId, AtmosphereRaw, VolcanismRaw)
				VALUES (601, 701, 'Carbon Dioxide', 'None'), (602, 702, 'Carbon Dioxide', 'None');
				INSERT INTO BodyTypes (Id, Name) VALUES (401, 'Rocky'), (402, 'HIGH METAL CONTENT BODY');
				INSERT INTO BioSpawnRuleBodyTypes (SpawnRuleId, BodyTypeId, Mode)
				VALUES (601, 401, 'Required'), (601, 402, 'Required');
				INSERT INTO Atmospheres (Id, Name) VALUES (501, 'Carbon Dioxide');
				INSERT INTO SpeciesAtmosphereConditions (SpeciesId, AtmosphereId, Mode)
				VALUES (701, 501, 'Required'), (702, 501, 'Required');
				""");
		}
		await using var db = CreateContext(connection);
		await new BioDataSeeder(db).SeedAsync();
		db.ChangeTracker.Clear();
		var arcus = await db.BioSpecies.SingleAsync(x => x.Name == "Aleoida Arcus");
		Assert.AreEqual(701, arcus.Id);
		Assert.AreEqual(801, arcus.GenusId);
		Assert.AreEqual("My Arcus", arcus.DisplayName);
		Assert.AreEqual(155, arcus.MinScanDistanceM);
		Assert.AreEqual(901, arcus.VariantDeterminantId);
		Assert.AreEqual(7252500, arcus.BaseValue);
		var rule = await db.BioSpawnRules.Include(x => x.BodyTypes).Include(x => x.Atmospheres)
			.SingleAsync(x => x.SpeciesId == 701);
		CollectionAssert.AreEquivalent(new[] { 401, 402 }, rule.BodyTypes.Select(x => x.BodyTypeId).ToArray());
		Assert.AreEqual(501, rule.Atmospheres.Single().AtmosphereId);
		Assert.AreEqual(116, await db.BioSpecies.CountAsync());
		Assert.IsTrue(await db.BioSpawnRules.AnyAsync(x => x.Id == 602 && x.SpeciesId == 702));
		Assert.IsTrue(await db.Set<BioSpawnRuleAtmosphere>().AnyAsync(x => x.SpawnRuleId == 602 && x.AtmosphereId == 501));
	}
}
