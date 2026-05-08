using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ED.Assistant.Migrations
{
    /// <inheritdoc />
    public partial class AddVariantDeterminants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.CreateTable(
			name: "VariantDeterminants",
			columns: table => new
			{
				Id = table.Column<int>(type: "INTEGER", nullable: false)
					.Annotation("Sqlite:Autoincrement", true),
				Name = table.Column<string>(type: "TEXT", nullable: false)
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_VariantDeterminants", x => x.Id);
			});

			migrationBuilder.CreateIndex(
				name: "IX_VariantDeterminants_Name",
				table: "VariantDeterminants",
				column: "Name",
				unique: true);

			migrationBuilder.Sql("""
			INSERT INTO VariantDeterminants (Name)
			SELECT DISTINCT TRIM(VariantDeterminant)
			FROM BioSpecies
			WHERE VariantDeterminant IS NOT NULL
			  AND TRIM(VariantDeterminant) <> '';
			""");

			migrationBuilder.Sql("""
			INSERT INTO VariantDeterminants (Name)
			SELECT 'Unknown'
			WHERE NOT EXISTS (
				SELECT 1 FROM VariantDeterminants WHERE Name = 'Unknown'
			);
			""");

			migrationBuilder.CreateTable(
				name: "BioSpecies_New",
				columns: table => new
				{
					Id = table.Column<int>(type: "INTEGER", nullable: false)
						.Annotation("Sqlite:Autoincrement", true),
					GenusId = table.Column<int>(type: "INTEGER", nullable: false),
					Name = table.Column<string>(type: "TEXT", nullable: false),
					DisplayName = table.Column<string>(type: "TEXT", nullable: false),
					BaseValue = table.Column<int>(type: "INTEGER", nullable: false),
					MinScanDistanceM = table.Column<int>(type: "INTEGER", nullable: false),
					VariantDeterminantId = table.Column<int>(type: "INTEGER", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_BioSpecies", x => x.Id);

					table.ForeignKey(
						name: "FK_BioSpecies_BioGenera_GenusId",
						column: x => x.GenusId,
						principalTable: "BioGenera",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);

					table.ForeignKey(
						name: "FK_BioSpecies_VariantDeterminants_VariantDeterminantId",
						column: x => x.VariantDeterminantId,
						principalTable: "VariantDeterminants",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.Sql("""
			INSERT INTO BioSpecies_New (
				Id,
				GenusId,
				Name,
				DisplayName,
				BaseValue,
				MinScanDistanceM,
				VariantDeterminantId
			)
			SELECT
				s.Id,
				s.GenusId,
				s.Name,
				s.DisplayName,
				s.BaseValue,
				s.MinScanDistanceM,
				COALESCE(v.Id, u.Id)
			FROM BioSpecies s
			LEFT JOIN VariantDeterminants v
				ON v.Name = TRIM(s.VariantDeterminant)
			CROSS JOIN VariantDeterminants u
			WHERE u.Name = 'Unknown';
			""");

			migrationBuilder.DropTable(
				name: "BioSpecies");

			migrationBuilder.RenameTable(
				name: "BioSpecies_New",
				newName: "BioSpecies");

			migrationBuilder.CreateIndex(
				name: "IX_BioSpecies_GenusId",
				table: "BioSpecies",
				column: "GenusId");

			migrationBuilder.CreateIndex(
				name: "IX_BioSpecies_VariantDeterminantId",
				table: "BioSpecies",
				column: "VariantDeterminantId");

			migrationBuilder.CreateIndex(
				name: "IX_BioSpecies_GenusId_Name",
				table: "BioSpecies",
				columns: ["GenusId", "Name"],
				unique: true);
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.AddColumn<string>(
			name: "VariantDeterminant",
			table: "BioSpecies",
			type: "TEXT",
			nullable: false,
			defaultValue: "");

			migrationBuilder.Sql("""
			UPDATE BioSpecies
			SET VariantDeterminant = (
				SELECT Name
				FROM VariantDeterminants
				WHERE VariantDeterminants.Id = BioSpecies.VariantDeterminantId
			);
			""");

			migrationBuilder.DropTable(
				name: "VariantDeterminants");
		}
    }
}
