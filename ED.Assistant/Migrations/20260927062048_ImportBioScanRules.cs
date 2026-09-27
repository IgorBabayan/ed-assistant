using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ED.Assistant.Migrations
{
    /// <inheritdoc />
    public partial class ImportBioScanRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BioSpawnRules_SpeciesId",
                table: "BioSpawnRules");

            migrationBuilder.AlterColumn<int>(
                name: "MinScanDistanceM",
                table: "BioSpecies",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "JournalName",
                table: "BioSpecies",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MaxGravityG",
                table: "BioSpawnRules",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MaxOrbitalPeriodExclusive",
                table: "BioSpawnRules",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "MaxOrbitalPeriodSeconds",
                table: "BioSpawnRules",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MaxPressureAtmospheres",
                table: "BioSpawnRules",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MaxPressureExclusive",
                table: "BioSpawnRules",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "MaxTemperatureK",
                table: "BioSpawnRules",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MinArrivalDistanceLs",
                table: "BioSpawnRules",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MinGravityG",
                table: "BioSpawnRules",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MinPressureAtmospheres",
                table: "BioSpawnRules",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MinTemperatureK",
                table: "BioSpawnRules",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nebula",
                table: "BioSpawnRules",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceFile",
                table: "BioSpawnRules",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceIndex",
                table: "BioSpawnRules",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VolcanismMode",
                table: "BioSpawnRules",
                type: "TEXT",
                nullable: false,
                defaultValue: "Unrestricted");

            migrationBuilder.AddColumn<string>(
                name: "JournalName",
                table: "BioGenera",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BioCatalogVersions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ContentHash = table.Column<string>(type: "TEXT", nullable: false),
                    SourceCommit = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BioCatalogVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BioSpawnRuleAtmosphereComponents",
                columns: table => new
                {
                    SpawnRuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    AtmosphereId = table.Column<int>(type: "INTEGER", nullable: false),
                    MinPercent = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BioSpawnRuleAtmosphereComponents", x => new { x.SpawnRuleId, x.AtmosphereId });
                    table.ForeignKey(
                        name: "FK_BioSpawnRuleAtmosphereComponents_Atmospheres_AtmosphereId",
                        column: x => x.AtmosphereId,
                        principalTable: "Atmospheres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BioSpawnRuleAtmosphereComponents_BioSpawnRules_SpawnRuleId",
                        column: x => x.SpawnRuleId,
                        principalTable: "BioSpawnRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BioSpawnRuleAtmospheres",
                columns: table => new
                {
                    SpawnRuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    AtmosphereId = table.Column<int>(type: "INTEGER", nullable: false),
                    Mode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BioSpawnRuleAtmospheres", x => new { x.SpawnRuleId, x.AtmosphereId, x.Mode });
                    table.ForeignKey(
                        name: "FK_BioSpawnRuleAtmospheres_Atmospheres_AtmosphereId",
                        column: x => x.AtmosphereId,
                        principalTable: "Atmospheres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BioSpawnRuleAtmospheres_BioSpawnRules_SpawnRuleId",
                        column: x => x.SpawnRuleId,
                        principalTable: "BioSpawnRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Move existing species-level conditions before removing their old table.
            migrationBuilder.Sql("""
                INSERT INTO BioSpawnRuleAtmospheres (SpawnRuleId, AtmosphereId, Mode)
                SELECT r.Id, a.AtmosphereId, a.Mode
                FROM SpeciesAtmosphereConditions a
                JOIN BioSpawnRules r ON r.SpeciesId = a.SpeciesId;
                """);
            migrationBuilder.DropTable(name: "SpeciesAtmosphereConditions");

            migrationBuilder.CreateTable(
                name: "BioSpawnRuleStars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SpawnRuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Scope = table.Column<string>(type: "TEXT", nullable: false),
                    StarType = table.Column<string>(type: "TEXT", nullable: false),
                    Luminosity = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BioSpawnRuleStars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BioSpawnRuleStars_BioSpawnRules_SpawnRuleId",
                        column: x => x.SpawnRuleId,
                        principalTable: "BioSpawnRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BioSpawnRuleSystemBodyTypes",
                columns: table => new
                {
                    SpawnRuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    BodyTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BioSpawnRuleSystemBodyTypes", x => new { x.SpawnRuleId, x.BodyTypeId });
                    table.ForeignKey(
                        name: "FK_BioSpawnRuleSystemBodyTypes_BioSpawnRules_SpawnRuleId",
                        column: x => x.SpawnRuleId,
                        principalTable: "BioSpawnRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BioSpawnRuleSystemBodyTypes_BodyTypes_BodyTypeId",
                        column: x => x.BodyTypeId,
                        principalTable: "BodyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BioSpawnRuleVolcanisms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SpawnRuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Pattern = table.Column<string>(type: "TEXT", nullable: false),
                    Match = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BioSpawnRuleVolcanisms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BioSpawnRuleVolcanisms_BioSpawnRules_SpawnRuleId",
                        column: x => x.SpawnRuleId,
                        principalTable: "BioSpawnRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BioSpecies_JournalName",
                table: "BioSpecies",
                column: "JournalName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BioSpawnRules_SpeciesId",
                table: "BioSpawnRules",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_BioGenera_JournalName",
                table: "BioGenera",
                column: "JournalName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BioSpawnRuleAtmosphereComponents_AtmosphereId",
                table: "BioSpawnRuleAtmosphereComponents",
                column: "AtmosphereId");

            migrationBuilder.CreateIndex(
                name: "IX_BioSpawnRuleAtmospheres_AtmosphereId",
                table: "BioSpawnRuleAtmospheres",
                column: "AtmosphereId");

            migrationBuilder.CreateIndex(
                name: "IX_BioSpawnRuleStars_SpawnRuleId",
                table: "BioSpawnRuleStars",
                column: "SpawnRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_BioSpawnRuleSystemBodyTypes_BodyTypeId",
                table: "BioSpawnRuleSystemBodyTypes",
                column: "BodyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BioSpawnRuleVolcanisms_SpawnRuleId",
                table: "BioSpawnRuleVolcanisms",
                column: "SpawnRuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // The previous schema cannot represent alternative rules. Do not silently
            // delete alternatives or merge mutually exclusive conditions on rollback.
            throw new System.NotSupportedException(
                "Restore a pre-import database backup to return to the single-rule schema.");
        }
    }
}
