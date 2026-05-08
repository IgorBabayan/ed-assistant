using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ED.Assistant.Migrations
{
    /// <inheritdoc />
    public partial class MoveSpawnRuleBodyTypesToRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.CreateTable(
			name: "BioSpawnRules_New",
			columns: table => new
			{
				Id = table.Column<int>(type: "INTEGER", nullable: false)
					.Annotation("Sqlite:Autoincrement", true),
				SpeciesId = table.Column<int>(type: "INTEGER", nullable: false),
				AtmosphereRaw = table.Column<string>(type: "TEXT", nullable: false),
				VolcanismRaw = table.Column<string>(type: "TEXT", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_BioSpawnRules", x => x.Id);

				table.ForeignKey(
					name: "FK_BioSpawnRules_BioSpecies_SpeciesId",
					column: x => x.SpeciesId,
					principalTable: "BioSpecies",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

			migrationBuilder.Sql("""
			INSERT INTO BioSpawnRules_New (
				Id,
				SpeciesId,
				AtmosphereRaw,
				VolcanismRaw
			)
			SELECT
				Id,
				SpeciesId,
				AtmosphereRaw,
				VolcanismRaw
			FROM BioSpawnRules;
			""");

			migrationBuilder.DropTable(name: "BioSpawnRules");

			migrationBuilder.RenameTable(
				name: "BioSpawnRules_New",
				newName: "BioSpawnRules");

			migrationBuilder.CreateIndex(
				name: "IX_BioSpawnRules_SpeciesId",
				table: "BioSpawnRules",
				column: "SpeciesId",
				unique: true);

			// New relation table
			migrationBuilder.CreateTable(
				name: "BioSpawnRuleBodyTypes",
				columns: table => new
				{
					SpawnRuleId = table.Column<int>(type: "INTEGER", nullable: false),
					BodyTypeId = table.Column<int>(type: "INTEGER", nullable: false),
					Mode = table.Column<string>(type: "TEXT", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey(
						"PK_BioSpawnRuleBodyTypes",
						x => new { x.SpawnRuleId, x.BodyTypeId, x.Mode });

					table.ForeignKey(
						name: "FK_BioSpawnRuleBodyTypes_BioSpawnRules_SpawnRuleId",
						column: x => x.SpawnRuleId,
						principalTable: "BioSpawnRules",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);

					table.ForeignKey(
						name: "FK_BioSpawnRuleBodyTypes_BodyTypes_BodyTypeId",
						column: x => x.BodyTypeId,
						principalTable: "BodyTypes",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_BioSpawnRuleBodyTypes_BodyTypeId",
				table: "BioSpawnRuleBodyTypes",
				column: "BodyTypeId");

			migrationBuilder.Sql("""
			INSERT INTO BioSpawnRuleBodyTypes (
				SpawnRuleId,
				BodyTypeId,
				Mode
			)
			SELECT
				sr.Id,
				sbtc.BodyTypeId,
				sbtc.Mode
			FROM BioSpawnRules sr
			JOIN SpeciesBodyTypeConditions sbtc
				ON sbtc.SpeciesId = sr.SpeciesId;
			""");

			migrationBuilder.DropTable(name: "SpeciesBodyTypeConditions");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.CreateTable(
			name: "SpeciesBodyTypeConditions",
			columns: table => new
			{
				SpeciesId = table.Column<int>(type: "INTEGER", nullable: false),
				BodyTypeId = table.Column<int>(type: "INTEGER", nullable: false),
				Mode = table.Column<string>(type: "TEXT", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey(
					"PK_SpeciesBodyTypeConditions",
					x => new { x.SpeciesId, x.BodyTypeId, x.Mode });

				table.ForeignKey(
					name: "FK_SpeciesBodyTypeConditions_BioSpecies_SpeciesId",
					column: x => x.SpeciesId,
					principalTable: "BioSpecies",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);

				table.ForeignKey(
					name: "FK_SpeciesBodyTypeConditions_BodyTypes_BodyTypeId",
					column: x => x.BodyTypeId,
					principalTable: "BodyTypes",
					principalColumn: "Id",
					onDelete: ReferentialAction.Restrict);
			});

			migrationBuilder.Sql("""
			INSERT INTO SpeciesBodyTypeConditions (
				SpeciesId,
				BodyTypeId,
				Mode
			)
			SELECT
				sr.SpeciesId,
				srbt.BodyTypeId,
				srbt.Mode
			FROM BioSpawnRuleBodyTypes srbt
			JOIN BioSpawnRules sr
				ON sr.Id = srbt.SpawnRuleId;
			""");

			migrationBuilder.DropTable(name: "BioSpawnRuleBodyTypes");

			migrationBuilder.AddColumn<string>(
				name: "BodyTypesRaw",
				table: "BioSpawnRules",
				type: "TEXT",
				nullable: false,
				defaultValue: "");
		}
    }
}
